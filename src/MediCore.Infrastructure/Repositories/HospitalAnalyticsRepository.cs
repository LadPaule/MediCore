using MediCore.Application.Interfaces;
using MediCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Infrastructure.Repositories;

public class HospitalAnalyticsRepository : IHospitalAnalyticsRepository
{
    private readonly AppDbContext _context;

    public HospitalAnalyticsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetPatientCount(){
        return await _context.Patients.CountAsync();
    }
    public async Task<int> GetPrescriptionCount(){
        return await _context.Prescriptions.CountAsync();
    }
    
    public async Task<List<object>> GetTopMedications()
    {
        return await _context.DispensedMedications
            .GroupBy(d => d.MedicationId)
            .Select(g => new { MedicationId = g.Key, TotalDispensed = g.Sum(x => x.Quantity) })
            .OrderByDescending(x => x.TotalDispensed)
            .Take(5)
            .Cast<object>()
            .ToListAsync();
    }

    public async Task<List<object>> GetLowStockMedications()
    {
        return await _context.PharmacyInventories
            .Where(i => i.QuantityInStock <= i.ReorderLevel)
            .Select(i => new { i.MedicationId, i.QuantityInStock, i.ReorderLevel })
            .Cast<object>()
            .ToListAsync();
    }
}

