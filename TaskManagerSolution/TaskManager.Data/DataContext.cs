using System.Collections.Generic;
using TaskManager.Data.Models;

namespace TaskManager.Data
{
    public class DataContext
    {
        private static List<Task> tasks = new List<Task>();
        private static int nextId = 1;

        public List<Task> GetTasks()
        {
            return tasks;
        }

        public void AddTask(Task task)
        {
            task.Id = nextId++;
            tasks.Add(task);
        }

        public void UpdateTask(Task task)
        {
            var index = tasks.FindIndex(t => t.Id == task.Id);
            if (index >= 0)
            {
                tasks[index] = task;
            }
        }
    }
}
