namespace MediCore.BlazorUI.Models.Pharmacy;

public class DispenseMedicineVM
{
    public Guid? PrescriptionId { get; set; }
    public Guid MedicationId { get; set; }
    public int Quantity { get; set; }
}
