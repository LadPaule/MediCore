public class PharmacyInventory
{
    public Guid Id { get; set; }

    public Guid MedicationId { get; set; }

    public string MedicationName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public DateTime ExpiryDate { get; set; }
}

