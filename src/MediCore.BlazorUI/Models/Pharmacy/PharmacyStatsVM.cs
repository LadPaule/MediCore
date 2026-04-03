namespace MediCore.BlazorUI.Models.Pharmacy;

public class PharmacyStatsVM
{
    public int TotalMedicines { get; set; }
    public int AvailableStock { get; set; }
    public int LowStock { get; set; }
    public int ExpiringSoon { get; set; }
}

