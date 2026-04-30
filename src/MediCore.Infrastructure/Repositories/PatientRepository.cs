using MediCore.Domain.Entities;
using MediCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediCore.Application.Interfaces;


namespace MediCore.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;
    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Patient> AddAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<List<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .Include(p => p.AssignedDoctor)
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(Guid id)
    {
        return await _context.Patients
            .Include(p => p.AssignedDoctor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Patient>> GetByDoctorAsync(string doctorId)
    {
        return await _context.Patients
            .Include(p => p.AssignedDoctor)
            .Where(p => p.AssignedDoctorId == doctorId)
            .ToListAsync();
    }

    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }
}
