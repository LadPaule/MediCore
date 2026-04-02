namespace MediCore.Domain.Entities;

public class StockTransaction
{
    public Guid Id { get; set; }
    public Guid MedicationId { get; set; }
    public int QuantityChanged { get; set; }
    public string TransactionType { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Medication Medication { get; set; } = default!;
}



