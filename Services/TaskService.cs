using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<TaskItem> CreateAsync(TaskItem task, int userId)
        {
            task.UserId = userId;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }
        public async Task<List<TaskItem>> GetAllAsync(int userId) => await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        public async Task<TaskItem?> GetByIdAsync(int id, int userId) => await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        public async Task<bool> UpdateAsync(int id, TaskItem task, int userId)
        {
            var existing = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (existing is null) return false;
            existing.Title = task.Title;
            existing.IsCompleted = task.IsCompleted;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var existing = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (existing is null) return false;
            _context.Tasks.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
