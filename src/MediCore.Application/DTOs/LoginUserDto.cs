using System.ComponentModel.DataAnnotations;

namespace MediCore.Application.DTOs;

public class LoginUserDto
{
    [Required]
    public required string Email {get; set;}

    [Required]
    public required string Password {get; set;}
}

