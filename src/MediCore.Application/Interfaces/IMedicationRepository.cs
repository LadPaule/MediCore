using MediCore.Domain.Entities;

namespace MediCore.Application.Interfaces;

public interface IPrescriptionRepository
{
    Task<Prescription> AddAsync(Prescription prescription);

    Task<List<Prescription>> GetByMedicalRecordIdAsync(Guid medicalRecordId);
}

