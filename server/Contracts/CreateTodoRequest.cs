namespace Server.Contracts;
using System.ComponentModel.DataAnnotations;

public sealed class CreateTodoRequest
{
    [Required, MinLength(1), MaxLength(200)]
    public string? Title { get; set; }
}
