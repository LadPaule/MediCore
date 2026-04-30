
namespace MediCore.Application.DTOs.Pharmacy;

public class MedicineDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; }
}

