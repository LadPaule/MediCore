using System.ComponentModel.DataAnnotations;

namespace MediCore.BlazorUI.Models;

public class RegisterUserRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = "";
}
