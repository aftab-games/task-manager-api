using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetAll();
        TaskItem? GetById(int id);
    }
}
