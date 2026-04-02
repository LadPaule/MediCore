using MediCore.Application.Interfaces;
using MediCore.Domain.Entities;
using MediCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace MediCore.Infrastructure.Repositories;

public class PharmacyRepository : IPharmacyRepository
{
    private readonly AppDbContext _context;
    public PharmacyRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<PharmacyInventory?> GetInventory(Guid medicationId)
    {
        return await _context.PharmacyInventories
        .FirstOrDefaultAsync(i => i.MedicationId == medicationId);
    }
    public async Task UpdateInventory(PharmacyInventory inventory)
    {
        _context.PharmacyInventories.Update(inventory);
        await _context.SaveChangesAsync();
    }
    public async Task AddDispensedMedication(DispensedMedication dispensed)
    {
        _context.DispensedMedications.Add(dispensed);
        await _context.SaveChangesAsync();
    }
    public async Task AddStockTransaction(StockTransaction transaction)
    {
        _context.StockTransactions.Add(transaction);
        await _context.SaveChangesAsync();
    }
}