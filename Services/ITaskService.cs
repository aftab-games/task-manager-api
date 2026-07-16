using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, TaskItem task);
        Task<bool> DeleteAsync(int id);
    }
}
