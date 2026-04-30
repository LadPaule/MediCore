namespace MediCore.BlazorUI.Models.Pharmacy;

public class MedicineVM
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.Date.AddYears(1);
}

public class CreateMedicineVM
{
    public string Name { get; set; } = "";
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; } = 100;
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.Date.AddYears(1);
    public string Description { get; set; } = "";
    public string Manufacturer { get; set; } = "";
}

public class MedicineDetailVM
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.Date.AddYears(1);
    public string Description { get; set; } = "";
    public string Manufacturer { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public List<StockTransactionVM> RecentTransactions { get; set; } = new();
}

public class StockTransactionVM
{
    public Guid Id { get; set; }
    public int QuantityChanged { get; set; }
    public string TransactionType { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class RestockVM
{
    public int Quantity { get; set; }
    public string Note { get; set; } = "";
}

public class AdjustStockVM
{
    public int NewQuantity { get; set; }
    public string Reason { get; set; } = "";
}

public class UpdateMedicineVM
{
    public string Name { get; set; } = "";
    public int ReorderLevel { get; set; } = 100;
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.Date.AddYears(1);
    public string Description { get; set; } = "";
    public string Manufacturer { get; set; } = "";
}
