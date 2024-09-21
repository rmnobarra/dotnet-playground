import os
import json
import requests
import time
import fnmatch
from pathlib import Path
import shutil
import pygit2

# Configurações
GH_TOKEN = os.environ['GH_TOKEN']
REPO_URL = 'https://github.com/rmnobarra/dotnet-playground'
WORK_DIR = './workdir'
MODERNIZATOR_DIR = os.path.join(WORK_DIR, 'modernizator')  # Diretório onde será criada a estrutura
AUTH_URL = 'https://idm.stackspot.com/growth-demo/oidc/oauth/token'
STACKSPOT_AI_URL = 'https://genai-code-buddy-api.stackspot.com/v1/quick-commands/create-execution/modernizator'
CALLBACK_URL = 'https://genai-code-buddy-api.stackspot.com/v1/quick-commands/callback'
CLIENT_ID = os.environ['CLIENT_ID']
CLIENT_SECRET = os.environ['CLIENT_SECRET']

# Função para obter o token de acesso via OAuth2
def get_access_token():
    headers = {
        'Content-Type': 'application/x-www-form-urlencoded'
    }
    data = {
        'client_id': CLIENT_ID,
        'grant_type': 'client_credentials',
        'client_secret': CLIENT_SECRET
    }
    
    response = requests.post(AUTH_URL, headers=headers, data=data)
    
    if response.status_code == 200:
        return response.json().get('access_token')
    else:
        raise Exception('Falha ao obter o token de acesso.')

# Função para enviar o JSON ao endpoint da StackSpot AI
def send_json_to_stackspot_ai(access_token, json_data):
    headers = {
        'Authorization': f'Bearer {access_token}',
        'Content-Type': 'application/json'
    }
    
    data = {
        'input_data': json_data
    }
    
    response = requests.post(STACKSPOT_AI_URL, headers=headers, json=data)
    
    if response.status_code in [200, 201]:  # Sucesso se for 200 ou 201
        conversation_id = response.text.strip('"')  # Limpar as aspas da resposta
        print(f'Requisição enviada com sucesso! ID da execução: {conversation_id}')
        return conversation_id
    else:
        raise Exception(f'Falha ao enviar dados: {response.status_code} - {response.text}')

# Função para realizar polling no callback
def poll_callback(conversation_id, access_token):
    headers = {
        'Authorization': f'Bearer {access_token}'
    }

    # A URL de callback agora inclui o execution_id diretamente no caminho
    callback_url = f'{CALLBACK_URL}/{conversation_id}'
    
    while True:
        response = requests.get(callback_url, headers=headers)
        
        if response.status_code == 200:
            result = response.json()
            # Exibir o resultado completo para análise
            print(f'Resposta do callback: {result}')

            # Verificar o status dentro de "progress"
            status = result.get('progress', {}).get('status')
            if status == 'COMPLETED':
                print('Execução concluída com sucesso!')
                print('Resultado final:', result)

                # Tentar encontrar o passo "plano-de-acao"
                modernize_step = next((step for step in result.get('steps', []) if step.get('step_name') == 'plano-de-acao'), None)
                
                if modernize_step is None:
                    print("O passo 'plano-de-acao' não foi encontrado.")
                    return

                # Extrair o markdown do resultado do passo "plano-de-acao"
                markdown_content = modernize_step['step_result']['answer']
                
                # Salvar o conteúdo markdown em um arquivo
                save_markdown_to_file(markdown_content, MODERNIZATOR_DIR, 'plano_de_modernizacao.md')

                # Criar a estrutura de arquivos e pastas com base no JSON (se houver)
                structure = result.get("input_data", {}).get("json", None)
                if structure:
                    print(f'Criando estrutura de arquivos no diretório {MODERNIZATOR_DIR}')
                    create_structure_from_json(structure, MODERNIZATOR_DIR)

                return result
            else:
                print(f'Status atual: {status}. Verificando novamente em 5 segundos.')
        elif response.status_code == 403:
            print(f'Erro 403 - Acesso negado. Verifique se o token tem as permissões corretas ou se a URL está correta.')
            print(f'Token utilizado: {access_token}')
            break
        else:
            print(f'Erro ao consultar o status: {response.status_code}')
        
        time.sleep(5)

# Função para salvar o conteúdo Markdown em disco
def save_markdown_to_file(content, directory, filename):
    # Criar o diretório se ele não existir
    if not os.path.exists(directory):
        os.makedirs(directory)

    file_path = os.path.join(directory, filename)
    
    # Salvar o conteúdo no arquivo
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(content)
    
    print(f'Markdown salvo com sucesso em: {file_path}')

# Função para salvar JSON no disco
def save_json_to_disk(data, filename):
    file_path = os.path.join(WORK_DIR, filename)
    with open(file_path, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=4)
    print(f'JSON salvo com sucesso em: {file_path}')

# Função para clonar o repositório
def clone_repo(repo_url, clone_dir, token):
    repo_url_with_auth = repo_url.replace("https://", f"https://{token}@")
    pygit2.clone_repository(repo_url_with_auth, clone_dir)
    print(f'Repositório clonado em {clone_dir}')

# Função para ler o .gitignore e converter os padrões em uma lista
def read_gitignore(repo_dir):
    gitignore_path = os.path.join(repo_dir, '.gitignore')
    ignored_patterns = []

    if os.path.exists(gitignore_path):
        with open(gitignore_path, 'r') as f:
            for line in f:
                line = line.strip()
                if line and not line.startswith('#'):
                    ignored_patterns.append(line)

    # Adicionar manualmente padrões como .git e .gitignore que devem ser ignorados sempre
    ignored_patterns.append('.git/')
    ignored_patterns.append('.gitignore')
    return ignored_patterns

# Função para verificar se um caminho deve ser ignorado
def is_ignored(path, ignored_patterns):
    for pattern in ignored_patterns:
        if fnmatch.fnmatch(path, pattern) or fnmatch.fnmatch(path, f'*/{pattern}') or fnmatch.fnmatch(path, f'{pattern}*'):
            return True
    return False

# Função para gerar JSON com a estrutura de diretórios e arquivos
def generate_json_structure(base_dir, ignored_patterns):
    def process_directory(directory):
        dir_structure = {"files": {}, "dirs": {}}
        
        for root, dirs, files in os.walk(directory):
            if '.git' in dirs:
                dirs.remove('.git')
            
            dirs[:] = [d for d in dirs if not is_ignored(os.path.relpath(os.path.join(root, d), base_dir), ignored_patterns)]

            rel_dir = os.path.relpath(root, base_dir)
            if is_ignored(rel_dir, ignored_patterns):
                return None
            
            for file_name in files:
                file_path = os.path.join(root, file_name)
                if not is_ignored(os.path.relpath(file_path, base_dir), ignored_patterns):
                    with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
                        content = f.read()
                    dir_structure["files"][file_name] = content
            
            for dir_name in dirs:
                dir_path = os.path.join(root, dir_name)
                sub_dir_structure = process_directory(dir_path)
                if sub_dir_structure is not None:
                    dir_structure["dirs"][dir_name] = sub_dir_structure

            break
        
        return dir_structure

    return process_directory(base_dir)

# Função para criar a estrutura de arquivos e diretórios com base no JSON
def create_structure_from_json(structure, base_dir):
    """Cria a estrutura de arquivos e diretórios com base no JSON fornecido."""
    # Criar o diretório base se ele não existir
    if not os.path.exists(base_dir):
        os.makedirs(base_dir)
        print(f'Diretório {base_dir} criado.')

    # Processar arquivos
    if 'files' in structure:
        for file_name, file_content in structure['files'].items():
            file_path = os.path.join(base_dir, file_name)
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(file_content)
            print(f'Arquivo criado: {file_path}')

    # Processar subdiretórios
    if 'dirs' in structure:
        for dir_name, dir_structure in structure['dirs'].items():
            sub_dir_path = os.path.join(base_dir, dir_name)
            create_structure_from_json(dir_structure, sub_dir_path)

# Função para copiar o arquivo markdown para o repositório clonado
def copy_markdown_to_repo(source_file, destination_dir):
    """Copia o arquivo markdown para o repositório clonado."""
    if not os.path.exists(source_file):
        print(f"Erro: O arquivo {source_file} não foi encontrado.")
        return False
    
    # Criar o diretório no repositório clonado, se não existir
    if not os.path.exists(destination_dir):
        os.makedirs(destination_dir)
    
    # Copiar o arquivo
    shutil.copy(source_file, destination_dir)
    print(f"Arquivo {source_file} copiado para {destination_dir}")
    return True

# Função para criar uma nova branch e fazer commit do arquivo no repositório
def create_branch_and_commit(repo_dir, branch_name, file_to_commit, commit_message):
    # Abrir o repositório clonado
    repo = pygit2.Repository(repo_dir)

    # Verificar se a branch já existe
    if branch_name in repo.branches.local:
        print(f'A branch {branch_name} já existe. Usando essa branch.')
    else:
        # Criar uma nova branch a partir da branch atual (tipicamente 'main' ou 'master')
        print(f'Criando nova branch: {branch_name}')
        main_branch = repo.lookup_reference('refs/heads/main') or repo.lookup_reference('refs/heads/master')
        repo.branches.create(branch_name, repo.head.peel())
        repo.checkout('refs/heads/' + branch_name)
        print(f'Branch {branch_name} criada e em uso.')

    # Caminho completo do arquivo a partir da raiz do repositório clonado
    file_full_path = os.path.join(repo_dir, file_to_commit)
    
    # Verificar se o arquivo existe no caminho correto
    if not os.path.exists(file_full_path):
        print(f'Erro: O arquivo {file_full_path} não foi encontrado.')
        return
    
    # Adicionar o arquivo ao índice (staging area)
    index = repo.index
    index.add(os.path.relpath(file_full_path, repo_dir))  # Usar o caminho relativo
    index.write()

    # Criar o commit
    author = pygit2.Signature('Seu Nome', 'seu-email@example.com')
    committer = pygit2.Signature('Seu Nome', 'seu-email@example.com')
    tree = index.write_tree()

    # Commitar na nova branch
    parent = repo.revparse_single('HEAD')
    commit = repo.create_commit(
        'refs/heads/' + branch_name,  # Branch onde será feito o commit
        author, committer, commit_message, tree, [parent.id]
    )
    print(f'Commit criado na branch {branch_name} com o arquivo {file_to_commit}')

    # Fazer o push da nova branch para o repositório remoto
    remote = repo.remotes['origin']
    remote.credentials = pygit2.UserPass(GH_TOKEN, '')
    remote.push([f'refs/heads/{branch_name}'])
    print(f'Branch {branch_name} enviada para o repositório remoto.')


# função principal
def main():
    # Obter token de acesso
    access_token = get_access_token()
    print(f'Token de acesso obtido: {access_token}')
    
    # Criar o diretório workdir, se não existir
    if not os.path.exists(WORK_DIR):
        os.makedirs(WORK_DIR)
        print(f'Diretório {WORK_DIR} criado com sucesso.')
    
    # Definir o caminho de clonagem dentro do workdir
    clone_dir = os.path.join(WORK_DIR, 'repo_clone')

    # Clonar o repositório
    clone_repo(REPO_URL, clone_dir, GH_TOKEN)
    
    # Lê os padrões do .gitignore
    ignored_patterns = read_gitignore(clone_dir)
    
    # Gerar JSON com a estrutura de diretórios e arquivos
    repo_structure = generate_json_structure(clone_dir, ignored_patterns)
    
    # Salvar o JSON da estrutura do repositório local em disco
    save_json_to_disk(repo_structure, 'estrutura_repositorio.json')
    
    # Estrutura para o JSON ajustado
    final_json = {
        "input_data": {
            "json": repo_structure
        }
    }

    # Enviar o JSON gerado para o StackSpot AI
    conversation_id = send_json_to_stackspot_ai(access_token, final_json)

    # Realizar polling no callback e criar a estrutura de arquivos e pastas com base no JSON
    if conversation_id:
        poll_callback(conversation_id, access_token)

        # Após salvar o arquivo markdown, adicionar à branch
        markdown_file = os.path.join(MODERNIZATOR_DIR, 'plano_de_modernizacao.md')  # Caminho do markdown gerado
        repo_markdown_dir = os.path.join(clone_dir, 'modernizator')  # Diretório no repositório clonado

        # Copiar o arquivo markdown para o repositório clonado
        if copy_markdown_to_repo(markdown_file, repo_markdown_dir):
            branch_name = 'modernizacao-plano'
            commit_message = 'Adicionando plano de modernização gerado automaticamente'
            create_branch_and_commit(clone_dir, branch_name, os.path.join('modernizator', 'plano_de_modernizacao.md'), commit_message)

if __name__ == '__main__':
    main()
