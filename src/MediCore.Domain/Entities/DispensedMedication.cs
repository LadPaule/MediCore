namespace MediCore.Domain.Entities;

public class DispensedMedication
{
    public Guid Id { get; set; }
    public Guid? PrescriptionId { get; set; }
    public Guid MedicationId { get; set; }
    public int Quantity { get; set; }
    public DateTime DispensedAt { get; set; } = DateTime.UtcNow;
    public Prescription? Prescription { get; set; }
    public Medication Medication { get; set; } = null!;
}
