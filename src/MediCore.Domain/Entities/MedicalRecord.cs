namespace MediCore.Domain.Entities;

public class MedicalRecord
{
    public Guid Id {get; set;}
    public Guid PatientId {get; set;}
    public Guid AppointmentId {get; set;}
    public string DoctorId {get; set;} =default!;
    public string Diagnosis {get; set;} =default!;
    public string Symptoms {get; set;} =default!;
    public string Notes {get; set;} =default!;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public Patient Patient {get; set;} =default!;
    public Appointment Appointment {get; set;} =default!;
    public ApplicationUser Doctor {get; set;} =default!;

}