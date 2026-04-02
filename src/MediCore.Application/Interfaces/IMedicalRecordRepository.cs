using MediCore.Domain.Entities;

namespace MediCore.Application.Interfaces;


public interface IMedicalRecordRepository
{
    Task<MedicalRecord> AddAsync(MedicalRecord record);
    Task<List<MedicalRecord>> GetByPatientIdAsync(Guid patientId);
    Task<List<MedicalRecord>> GetByAppointmentIdAsync(Guid appointmentId);
}