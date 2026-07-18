using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        Task<TaskItem> CreateAsync(TaskItem task, int userId);
        Task<List<TaskItem>> GetAllAsync(int userId);
        Task<TaskItem?> GetByIdAsync(int id, int userId);
        Task<bool> UpdateAsync(int id, TaskItem task, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
