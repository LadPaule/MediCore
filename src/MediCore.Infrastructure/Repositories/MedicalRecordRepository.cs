using MediCore.Application.Interfaces;
using MediCore.Domain.Entities;
using MediCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Infrastructure.Repositories;

public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly AppDbContext _context;

    public MedicalRecordRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<MedicalRecord> AddAsync(MedicalRecord record)
    {
        _context.MedicalRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }
    public async Task<List<MedicalRecord>> GetByPatientIdAsync(Guid patientId)
    {
        return await _context.MedicalRecords.Where(r => r.PatientId == patientId)
        .ToListAsync();
    }
    public async Task<List<MedicalRecord>> GetByAppointmentIdAsync(Guid appointmentId)
    {
        return await _context.MedicalRecords
        .Where(r => r.AppointmentId == appointmentId).ToListAsync();
    }
}