using System.ComponentModel.DataAnnotations;

namespace MediCore.Application.DTOs;

public class CreateAppointmentDto
{
    [Required]
    public Guid PatientId { get; set; }

    [Required]
    public string DoctorId { get; set; } = default!;

    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public string Reason { get; set; } = default!;
}
