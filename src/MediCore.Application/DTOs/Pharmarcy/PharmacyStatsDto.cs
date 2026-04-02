
namespace MediCore.Application.DTOs.Pharmacy;

public class PharmacyStatsDto
{
    public int TotalMedicines { get; set; }
    public int AvailableStock { get; set; }
    public int LowStock { get; set; }
    public int ExpiringSoon { get; set; }
}