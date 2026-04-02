using System.ComponentModel.DataAnnotations;

namespace MediCore.Domain.Entities;

public class AppUser
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = default!;

    [Required]
    public string Email { get; set; } = default!;

    public string Role { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}