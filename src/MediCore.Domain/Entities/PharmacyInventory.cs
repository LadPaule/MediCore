namespace MediCore.Domain.Entities;

public class PharmacyInventory
{
    public Guid Id {get; set;}
    public Guid MedicationId {get; set;}
    public int QuantityInStock {get; set;}
    public int ReorderLevel {get; set;}
    public DateTime LastUpdated {get; set;} = DateTime.UtcNow;
    public Medication Medication {get; set;} = default!;
}
