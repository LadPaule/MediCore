using MediCore.Application.DTOs;
using MediCore.Application.Interfaces;
using MediCore.Domain.Entities;

namespace  MediCore.Application.Services;

public class PatientService
{
    private readonly IPatientRepository _patientRepository;
    public PatientService (IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
    public async Task<Patient> CreatePatient(CreatePatientDto dto)
    {
        var patient = new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth.Kind == DateTimeKind.Utc
                ? dto.DateOfBirth
                : DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc),
            Gender = dto.Gender,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Address = dto.Address,
            AssignedDoctorId = string.IsNullOrWhiteSpace(dto.AssignedDoctorId) ? null : dto.AssignedDoctorId
        };
        return await _patientRepository.AddAsync(patient);
    }

    public async Task<List<Patient>> GetPatients()
    {
        return await _patientRepository.GetAllAsync();
    }

    public async Task<List<Patient>> GetPatientsByDoctor(string doctorId)
    {
        return await _patientRepository.GetByDoctorAsync(doctorId);
    }

    public async Task<Patient?> GetPatient(Guid id)
    {
        return await _patientRepository.GetByIdAsync(id);
    }

    public async Task<Patient?> AssignDoctor(Guid patientId, string? doctorId)
    {
        var patient = await _patientRepository.GetByIdAsync(patientId);
        if (patient == null) return null;

        patient.AssignedDoctorId = string.IsNullOrWhiteSpace(doctorId) ? null : doctorId;
        await _patientRepository.UpdateAsync(patient);
        return patient;
    }
}
