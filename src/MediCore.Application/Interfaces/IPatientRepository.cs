using MediCore.Domain.Entities;

namespace MediCore.Application.Interfaces;

public interface IPatientRepository
{
    Task<Patient> AddAsync(Patient patient);
    Task<List<Patient>> GetAllAsync();
    Task<Patient?> GetByIdAsync(Guid id);
    Task<List<Patient>> GetByDoctorAsync(string doctorId);
    Task UpdateAsync(Patient patient);
}
