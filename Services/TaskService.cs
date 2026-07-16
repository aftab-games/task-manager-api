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
        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }
        public async Task<List<TaskItem>> GetAllAsync() => await _context.Tasks.ToListAsync();
        public async Task<TaskItem?> GetByIdAsync(int id) => await _context.Tasks.FindAsync(id);
        public async Task<bool> UpdateAsync(int id, TaskItem task)
        {
            var existing = await _context.Tasks.FindAsync(id);
            if (existing is null) return false;
            existing.Title = task.Title;
            existing.IsCompleted = task.IsCompleted;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Tasks.FindAsync(id);
            if(existing is null) return false;
            _context.Tasks.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
