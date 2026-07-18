using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerApi.Models;
using TaskManagerApi.Services;
using TaskManagerApi.DTOs;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var task = new TaskItem { Title = dto.Title };
            var created = await _taskService.CreateAsync(task, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _taskService.GetAllAsync(GetUserId()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id, GetUserId());
            if (task is null) return NotFound();
            return Ok(task);
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
        {
            var task = new TaskItem { Title = dto.Title, IsCompleted = dto.IsCompleted };
            var success = await _taskService.UpdateAsync(id, task, GetUserId());
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _taskService.DeleteAsync(id, GetUserId());
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
