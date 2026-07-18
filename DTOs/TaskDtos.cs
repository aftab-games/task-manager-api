namespace TaskManagerApi.DTOs
{
    public record CreateTaskDto(string Title);
    public record UpdateTaskDto(string Title, bool IsCompleted);
}
