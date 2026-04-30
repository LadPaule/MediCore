using MediCore.Application.DTOs;
using MediCore.Application.Interfaces;
using MediCore.Domain.Entities;

namespace MediCore.Application.Services;

public class PrescriptionService
{
    private readonly IPrescriptionRepository _repository;

    public PrescriptionService(IPrescriptionRepository repository)
    {
        _repository = repository;
    }
    public async Task<Prescription> CreatePrescription(CreatePrescriptionDto dto)
    {
        var prescription = new Prescription
        {
            MedicalRecordId = dto.MedicalRecordId,
            DoctorId = dto.DoctorId,
            Items = dto.Items.Select(i => new PrescriptionItem
            {
                MedicationName = i.MedicationName,
                Dosage = i.Dosage,
                Frequency = i.Frequency,
                DurationDays = i.DurationDays
            }).ToList()
        };
        return await _repository.AddAsync(prescription);
    }

}


