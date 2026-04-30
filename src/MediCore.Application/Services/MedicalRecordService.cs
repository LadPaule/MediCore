using MediCore.Application.DTOs;
using MediCore.Application.Interfaces;
using MediCore.Domain.Entities;

namespace MediCore.Application.Services;

public class MedicalRecordService
{
    private readonly IMedicalRecordRepository _repository;

    public MedicalRecordService(IMedicalRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<MedicalRecord> CreateRecord(CreateMedicalRecordDto dto)
    {
        var record = new MedicalRecord
        {
            PatientId = dto.PatientId,
            AppointmentId = dto.AppointmentId,
            DoctorId = dto.DoctorId,
            Diagnosis = dto.Diagnosis,
            Symptoms = dto.Symptoms,
            Notes = dto.Notes
        };

        return await _repository.AddAsync(record);
    }
    public async Task<List<MedicalRecord>> GetPatientRecords(Guid patientId)
    {
        return await _repository.GetByPatientIdAsync(patientId);
    }
}