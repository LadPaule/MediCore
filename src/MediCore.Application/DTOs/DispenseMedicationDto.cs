namespace MediCore.Application.DTOs;

public class DispenseMedicationDto
{
    public Guid? PrescriptionId { get; set; }
    public Guid MedicationId { get; set; }
    public int Quantity { get; set; }
}
