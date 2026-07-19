using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.DTOs
{
    public record CreateTaskDto([Required, MinLength(1), MaxLength(200)] string Title);
    public record UpdateTaskDto([Required, MinLength(1), MaxLength(200)] string Title, bool IsCompleted);
}
