using System.Web.Mvc;
using TaskManager.Data;
using TaskManager.Data.Models;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers
{
    public class TasksController : Controller
    {
        private DataContext context = new DataContext();

        public ActionResult Index()
        {
            var tasks = context.GetTasks();
            return View(tasks);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = new Task
                {
                    Title = model.Title,
                    Description = model.Description,
                    IsCompleted = false,
                    CreatedAt = System.DateTime.Now
                };
                context.AddTask(task);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var task = context.GetTasks().Find(t => t.Id == id);
            if (task == null)
            {
                return HttpNotFound();
            }
            var model = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = new Task
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    IsCompleted = model.IsCompleted,
                    CreatedAt = System.DateTime.Now
                };
                context.UpdateTask(task);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
