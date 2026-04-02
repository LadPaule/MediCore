using System.ComponentModel.DataAnnotations;

namespace MediCore.Application.DTOs;

public class CreatePatientDto
{
    [Required]
    public required string FirstName {get; set;}
    [Required]
    public required string LastName {get; set;}
    [Required]
    public required DateTime DateOfBirth {get; set;}
    [Required]
    public required string Gender {get; set;}
    [Required]
    public required string PhoneNumber {get; set;}
    [Required]
    public required string Address {get; set;}

}