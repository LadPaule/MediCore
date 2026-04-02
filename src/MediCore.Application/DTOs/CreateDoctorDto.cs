namespace MediCore.Application.DTOs;

public class Doctor
{
    public Guid Id {get; set;}
    public  string FirstName {get; set;}="";
    public  string LastName {get; set;}="";
    public  string Speciality {get; set;}="";
    public  string Email {get; set;}="";
    public Guid DepartmentId {get; set;}

}