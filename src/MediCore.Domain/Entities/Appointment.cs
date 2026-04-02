namespace MediCore.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
public class Appointment
{
    public Guid Id {get; set;}
    public Guid PatientId {get; set;}
    public string DoctorId {get; set;} = default!;
    public DateTime AppointmentDate {get; set;}
    public string Reason {get; set;} = default!;
    public string Status {get; set;} = "Scheduled";
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public Patient Patient {get; set;} = default!;
    
    [ForeignKey("DoctorId")]
    public ApplicationUser Doctor {get; set;} = default!;
}
