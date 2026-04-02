using System.ComponentModel.DataAnnotations;

namespace MediCore.Application.DTOs;

public class CreateMedicalRecordDto
{
    [Required]
    public Guid PatientId {get; set;}
    [Required]
    public Guid AppointmentId {get; set;}

    [Required]
    public string DoctorId {get; set;} = default!;

    [Required]
    public string Diagnosis {get; set;} =default!;

    [Required]
    public string Symptoms {get; set;} = default!;
    public string Notes {get; set;} = default!;

}