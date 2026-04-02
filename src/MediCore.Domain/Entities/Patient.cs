namespace MediCore.Domain.Entities;


public class Patient
{
    public Guid Id {get; set;}
    public required string FirstName {get; set;}
    public required string LastName {get; set;}
    public DateTime? DateOfBirth {get; set;}
    public required string Gender {get; set;}
    public required string PhoneNumber {get; set;}
    public string Email {get; set;} = default!;

    public required string Address {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}