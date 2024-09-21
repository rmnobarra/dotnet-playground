# dotnet playground

## Coleção de Projetos .NET Framework 4.8

Este repositório contém quatro aplicações desenvolvidas em .NET Framework 4.8, cada uma explorando diferentes tecnologias e padrões de projeto. Os projetos são:

- **TaskManagerSolution**
- **CalculatorSolution**
- **PaintSolution**
- **MessageBoardSolution**

## 1. TaskManagerSolution

### Descrição

Uma aplicação ASP.NET MVC chamada TaskManager que permite gerenciar uma lista de tarefas (ToDo List). A aplicação suporta operações de criação, leitura e atualização de tarefas.

### Estrutura do Projeto

- **TaskManagerSolution.sln**: Arquivo de solução que agrupa os projetos.
- **TaskManager.Web**: Projeto ASP.NET MVC que serve como interface web.
  - **Controllers/TasksController.cs**: Controlador responsável pelas operações de tarefas.
  - **Models/TaskViewModel.cs**: Modelo de visão para a interação com as views.
  - **Views/Tasks**: Diretório contendo as views `Index.cshtml`, `Create.cshtml` e `Edit.cshtml`.
  - **Global.asax e Global.asax.cs**: Arquivos de configuração da aplicação.
  - **Web.config**: Arquivo de configuração do aplicativo web.
- **TaskManager.Data**: Biblioteca de classes que contém os modelos e a lógica de dados.
  - **Models/Task.cs**: Modelo de dados da tarefa.
  - **DataContext.cs**: Contexto de dados simulando um repositório em memória.
  - **TaskManager.Data.csproj**: Arquivo de projeto da biblioteca.

### Funcionalidades

- Listar tarefas existentes.
- Criar novas tarefas.
- Editar tarefas existentes.
- Separação de responsabilidades usando uma biblioteca de classes para lógica de dados.

### Como Compilar e Executar

1. Navegue até o diretório raiz `TaskManagerSolution`.

2. Execute o comando:

    ```bash
    msbuild TaskManagerSolution.sln
    ```

3. Após a compilação, hospede o aplicativo usando o IIS Express:

    ```bash
    iisexpress /path:"caminho\para\TaskManager.Web" /port:8080
    ```

4. Acesse [http://localhost:8080](http://localhost:8080) no navegador.

## 2. CalculatorSolution

### Descrição

Uma aplicação Windows Forms chamada SimpleCalculator que realiza operações matemáticas básicas: adição, subtração, multiplicação e divisão. A lógica de cálculo é separada em uma biblioteca de classes.

### Estrutura do Projeto

- **CalculatorSolution.sln**: Arquivo de solução que agrupa os projetos.
- **CalculatorApp**: Projeto Windows Forms que serve como interface gráfica.
  - **Forms/MainForm.cs**: Lógica da interface do usuário.
  - **Forms/MainForm.Designer.cs**: Código gerado automaticamente para a interface.
  - **Program.cs**: Ponto de entrada da aplicação.
  - **CalculatorApp.csproj**: Arquivo de projeto da aplicação.
- **CalculatorLib**: Biblioteca de classes que contém a lógica de cálculo.
  - **Calculator.cs**: Classe que implementa as operações matemáticas.
  - **CalculatorLib.csproj**: Arquivo de projeto da biblioteca.

### Funcionalidades

- Interface gráfica para inserir operandos.
- Botões para selecionar a operação desejada.
- Exibição do resultado da operação.
- Tratamento de erros, como divisão por zero.

### Como Compilar e Executar

1. Navegue até o diretório raiz `CalculatorSolution`.

2. Execute o comando:

    ```bash
    msbuild CalculatorSolution.sln
    ```

3. Após a compilação, vá para o diretório `CalculatorApp\bin\Debug`.

4. Execute o arquivo `CalculatorApp.exe`.

## 3. PaintSolution

### Descrição

Uma aplicação WPF (Windows Presentation Foundation) chamada SimplePaint que permite ao usuário desenhar formas básicas (linhas, retângulos e elipses) em um canvas. A lógica de desenho das formas é separada em uma biblioteca de classes.

### Estrutura do Projeto

- **PaintSolution.sln**: Arquivo de solução que agrupa os projetos.
- **SimplePaintApp**: Projeto WPF que serve como interface gráfica.
  - **App.xaml e App.xaml.cs**: Definição da aplicação WPF.
  - **MainWindow.xaml**: Interface do usuário com toolbar e canvas.
  - **MainWindow.xaml.cs**: Lógica para manipulação dos eventos de desenho.
  - **SimplePaintApp.csproj**: Arquivo de projeto da aplicação.
- **DrawingLib**: Biblioteca de classes que contém as definições das formas.
  - **Shapes/Shape.cs**: Classe base abstrata para as formas.
  - **Shapes/LineShape.cs**: Implementação da forma de linha.
  - **Shapes/RectangleShape.cs**: Implementação da forma de retângulo.
  - **Shapes/EllipseShape.cs**: Implementação da forma de elipse.
  - **DrawingLib.csproj**: Arquivo de projeto da biblioteca.

### Funcionalidades

- Seleção de formas para desenhar (linha, retângulo, elipse).
- Desenho interativo no canvas usando o mouse.
- Atualização em tempo real da forma durante o desenho.
- Separação da lógica de desenho em uma biblioteca de classes.

### Como Compilar e Executar

1. Navegue até o diretório raiz `PaintSolution`.

2. Execute o comando:

    ```bash
    msbuild PaintSolution.sln
    ```

3. Após a compilação, vá para o diretório `SimplePaintApp\bin\Debug`.

4. Execute o arquivo `SimplePaintApp.exe`.

## 4. MessageBoardSolution

### Descrição

Uma aplicação ASP.NET Web API chamada MessageBoardAPI que fornece uma API RESTful para um mural de mensagens. A API permite operações CRUD (criação, leitura, atualização e exclusão) de mensagens.

### Estrutura do Projeto

- **MessageBoardSolution.sln**: Arquivo de solução que agrupa os projetos.
- **MessageBoard.API**: Projeto ASP.NET Web API que expõe os endpoints da API.
  - **Controllers/MessagesController.cs**: Controlador que manipula as requisições HTTP.
  - **App_Start/WebApiConfig.cs**: Configuração das rotas da API.
  - **Global.asax e Global.asax.cs**: Configuração da aplicação.
  - **Web.config**: Arquivo de configuração do aplicativo web.
  - **MessageBoard.API.csproj**: Arquivo de projeto da aplicação.
- **MessageBoard.Data**: Biblioteca de classes que contém os modelos e a lógica de dados.
  - **Models/Message.cs**: Modelo de dados da mensagem.
  - **Repositories/MessageRepository.cs**: Repositório simulando acesso a dados.
  - **MessageBoard.Data.csproj**: Arquivo de projeto da biblioteca.

### Funcionalidades

- Endpoints RESTful para operações CRUD:
  - `GET /api/messages`: Lista todas as mensagens.
  - `GET /api/messages/{id}`: Obtém uma mensagem específica.
  - `POST /api/messages`: Cria uma nova mensagem.
  - `PUT /api/messages/{id}`: Atualiza uma mensagem existente.
  - `DELETE /api/messages/{id}`: Exclui uma mensagem.
- Serialização JSON das respostas.
- Separação da lógica de dados em uma biblioteca de classes.

### Como Compilar e Executar

1. Navegue até o diretório raiz `MessageBoardSolution`.

2. Execute o comando:

    ```bash
    msbuild MessageBoardSolution.sln
    ```

3. Após a compilação, hospede o aplicativo usando o IIS Express:

    ```bash
    iisexpress /path:"caminho\para\MessageBoard.API" /port:8080
    ```

4. Utilize ferramentas como Postman ou curl para interagir com a API:

    - Listar mensagens:

        ```bash
        curl -X GET http://localhost:8080/api/messages
        ```

    - Criar uma nova mensagem:

        ```bash
        curl -X POST http://localhost:8080/api/messages -H "Content-Type: application/json" -d "{\"Author\":\"John Doe\",\"Content\":\"Hello World!\"}"
        ```

### Considerações Gerais

#### Requisitos

- .NET Framework 4.8 instalado.
- MSBuild disponível no sistema para compilação.
- IIS Express ou outro servidor web compatível para hospedar as aplicações web.

#### Ferramentas de Teste (opcional)

- Postman ou curl para testar a API RESTful.

### Como Clonar e Configurar os Projetos

1. Clonar o Repositório:

    ```bash
    git clone https://github.com/rmnobarra/dotnet-playground.git
    ```

2. Navegar até cada solução e seguir as instruções de compilação e execução específicas.

### Sobre a Organização dos Projetos

- **Separação de Responsabilidades**: Em todos os projetos, há uma clara separação entre a interface do usuário e a lógica de negócios ou de dados, facilitando a manutenção e escalabilidade.
- **Dependências Entre Projetos**: As soluções demonstram como configurar dependências entre projetos, utilizando bibliotecas de classes compartilhadas.
- **Construção Manual**: Os projetos foram construídos para serem compilados e executados usando apenas um editor de texto comum e ferramentas de linha de comando, enfatizando a compreensão da estrutura e configuração dos projetos .NET.

### Contatos

Para dúvidas ou sugestões, sinta-se à vontade para entrar em contato:

- **Nome**: [Leonardo aka rmnobarra]
- **Email**: [rmnobarra@gmail.com]