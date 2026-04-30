using System.ComponentModel.DataAnnotations;

namespace MediCore.Application.DTOs.Pharmacy;

public class CreateMedicineDto
{
    [Required]
    public required string Name { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

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
