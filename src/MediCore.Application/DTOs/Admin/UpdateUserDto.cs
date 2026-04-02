namespace MediCore.Application.DTOs;
public class UpdateUserDto
{
    public required string  FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }
}