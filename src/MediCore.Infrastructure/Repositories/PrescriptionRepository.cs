using MediCore.Application.Interfaces;
using MediCore.Domain.Entities;
using MediCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace MediCore.Infrastructure.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly AppDbContext _context;

    public PrescriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Prescription> AddAsync(Prescription prescription)
    {
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
        return prescription;
    }

    public async Task<List<Prescription>> GetByMedicalRecordIdAsync(Guid medicalRecordId)
    {
        return await _context.Prescriptions.Include(p => p.Items)
        .Where(p => p.MedicalRecordId == medicalRecordId)
        .ToListAsync();
    }
}
