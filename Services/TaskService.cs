using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public class TaskService: ITaskService
    {
        private readonly List<TaskItem> _tasks = new()
        {
            new TaskItem { Id = 1, Title = "Learn ASP.NET Core DI", IsCompleted = false },
            new TaskItem { Id = 2, Title = "Build TaskManagerApi", IsCompleted = false }
        };
        public List<TaskItem> GetAll() => _tasks;
        public TaskItem? GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

    }
}
