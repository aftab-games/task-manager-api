using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;
using TaskManagerApi.Services;
using Xunit;

namespace TaskManagerApi.Tests;

public class TaskServiceTests
{
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_AddsTaskToDatabase()
    {
        var context = GetInMemoryContext();
        var service = new TaskService(context);
        var task = new TaskItem { Title = "Test Task" };

        var result = await service.CreateAsync(task, userId: 1);

        Assert.NotEqual(0, result.Id);
        Assert.Equal(1, result.UserId);
        Assert.Equal(1, context.Tasks.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenTaskBelongsToDifferentUser()
    {
        var context = GetInMemoryContext();
        var service = new TaskService(context);
        var task = await service.CreateAsync(new TaskItem { Title = "User 1's Task" }, userId: 1);

        var result = await service.GetByIdAsync(task.Id, userId: 2);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTask_WhenOwnedByCorrectUser()
    {
        var context = GetInMemoryContext();
        var service = new TaskService(context);
        var task = await service.CreateAsync(new TaskItem { Title = "My Task" }, userId: 1);

        var result = await service.GetByIdAsync(task.Id, userId: 1);

        Assert.NotNull(result);
        Assert.Equal("My Task", result!.Title);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenTaskDoesNotExist()
    {
        var context = GetInMemoryContext();
        var service = new TaskService(context);

        var result = await service.DeleteAsync(id: 999, userId: 1);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenTaskBelongsToDifferentUser()
    {
        var context = GetInMemoryContext();
        var service = new TaskService(context);
        var task = await service.CreateAsync(new TaskItem { Title = "User 1's Task" }, userId: 1);

        var updateAttempt = new TaskItem { Title = "Hacked Title", IsCompleted = true };
        var result = await service.UpdateAsync(task.Id, updateAttempt, userId: 2);

        Assert.False(result);
    }

    [Fact]
    public async Task GetAllAsync_OnlyReturnsTasksOwnedByRequestingUser()
    {
        var context = GetInMemoryContext();
        var service = new TaskService(context);
        await service.CreateAsync(new TaskItem { Title = "User 1 Task A" }, userId: 1);
        await service.CreateAsync(new TaskItem { Title = "User 1 Task B" }, userId: 1);
        await service.CreateAsync(new TaskItem { Title = "User 2 Task" }, userId: 2);

        var user1Tasks = await service.GetAllAsync(userId: 1);

        Assert.Equal(2, user1Tasks.Count);
        Assert.All(user1Tasks, t => Assert.Equal(1, t.UserId));
    }
}