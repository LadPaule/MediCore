namespace MediCore.Domain.Entities;

public class Medication
{
    public Guid Id {get; set;}
    public string Name {get; set;} = default!;
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; } = 100;
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Description {get; set;} = default!;
    public string Manufacturer {get; set;} = default!;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public ICollection<DispensedMedication> DispensedMedications { get; set; }
        = new List<DispensedMedication>();
    public ICollection<StockTransaction> StockTransactions { get; set; }
        = new List<StockTransaction>();
}




