namespace MediCore.BlazorUI.Models.Pharmacy;

public class MedicineVM
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; }
}

