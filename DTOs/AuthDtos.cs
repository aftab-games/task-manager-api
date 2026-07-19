using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.DTOs
{
    public record RegisterRequest(
        [Required, MinLength (3), MaxLength(30)]string Username, 
        [Required, MinLength(6)] string Password
        );
    public record LoginRequest(
        [Required]string Username, 
        [Required] string Password
        );
}
