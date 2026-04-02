using MediCore.Application.Caching;
using MediCore.Application.Interfaces;
using MediCore.Domain.Entities;
using MediCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace MediCore.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context, RedisCacheService cache)
    {
        _context = context;
    }
    public async Task <Appointment> AddAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }
    public async Task <List<Appointment>> GetAllAsync()
    {
        return await _context.Appointments
        .Include(a => a.Patient).ToListAsync();
    }
// Todo: Changed GetByPatientIdAsync to GetByPatientAsync
    public async Task<List<Appointment>> GetByPatientAsync(Guid patientId)
    {
        return await _context.Appointments
        .Where(a=> a.PatientId == patientId)
        .ToListAsync();
    }
    public async Task<List<Appointment>> GetByDoctorIdAsync(string doctorId)
    {
        return await _context.Appointments.Where(a => a.DoctorId == doctorId)
        .ToListAsync();
    }
    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }

     public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Appointments.FindAsync(id);

        if (entity != null)
        {
            _context.Appointments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}












