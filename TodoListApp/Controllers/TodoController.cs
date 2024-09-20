using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TodoListApp.Models;

namespace TodoListApp.Controllers
{
    public class TodoController : Controller
    {
        // Lista estática para simular um banco de dados
        private static List<TodoItem> todoItems = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Aprender ASP.NET MVC", IsCompleted = false },
            new TodoItem { Id = 2, Title = "Estudar .NET Framework", IsCompleted = false }
        };

        // Exibe a lista de tarefas
        public ActionResult Index()
        {
            return View(todoItems);
        }

        // Retorna a view para adicionar uma nova tarefa
        public ActionResult Create()
        {
            return View();
        }

        // Adiciona a nova tarefa na lista
        [HttpPost]
        public ActionResult Create(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                item.Id = todoItems.Count > 0 ? todoItems.Max(x => x.Id) + 1 : 1;
                todoItems.Add(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // Remove uma tarefa da lista
        public ActionResult Delete(int id)
        {
            var item = todoItems.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                todoItems.Remove(item);
            }
            return RedirectToAction("Index");
        }
    }
}
