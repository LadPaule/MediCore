
namespace MediCore.Application.Interfaces;


public interface IHospitalAnalyticsRepository
{
    Task<int> GetPatientCount();
    Task<int> GetPrescriptionCount();
    Task<List<object>> GetTopMedications();
    Task<List<object>> GetLowStockMedications();
}
