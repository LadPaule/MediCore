using MediCore.Application.Interfaces;

namespace MediCore.Application.Services.Analytics;

public class HospitalAnalyticsService // Fixed typo in class name
{
    private readonly IHospitalAnalyticsRepository _repository;

    public HospitalAnalyticsService(IHospitalAnalyticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> GetPatientCount() => await _repository.GetPatientCount();
    
    public async Task<int> GetPrescriptionCount() => await _repository.GetPrescriptionCount();

    public async Task<List<object>> GetTopMedications() => await _repository.GetTopMedications();

    public async Task<List<object>> GetLowStockMedications() => await _repository.GetLowStockMedications();
}