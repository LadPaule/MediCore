namespace MediCore.Domain.Entities;

public class Doctor
{
    public Guid Id {get; set;}
    public required string FirstName {get; set;}
    public required string LastName {get; set;}
    public required string Speciality {get; set;}
    public required string Email {get; set;}
    public Guid DepartmentId {get; set;}

}