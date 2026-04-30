using System.ComponentModel.DataAnnotations;

namespace MediCore.Application.DTOs.Pharmacy;

public class RestockMedicineDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    public string Note { get; set; } = string.Empty;
}

public class AdjustStockDto
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "New quantity must be zero or more.")]
    public int NewQuantity { get; set; }

    [Required]
    public string Reason { get; set; } = string.Empty;
}

public class UpdateMedicineDto
{
    [Required]
    public required string Name { get; set; }

    [Range(0, int.MaxValue)]
    public int ReorderLevel { get; set; } = 100;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }

    public string Description { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
}

public class MedicineDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Description { get; set; } = "";
    public string Manufacturer { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public List<StockTransactionDto> RecentTransactions { get; set; } = new();
}

public class StockTransactionDto
{
    public Guid Id { get; set; }
    public int QuantityChanged { get; set; }
    public string TransactionType { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
