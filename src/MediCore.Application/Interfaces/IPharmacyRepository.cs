using MediCore.Domain.Entities;

namespace MediCore.Application.Interfaces;

public interface IPharmacyRepository
{
    Task<PharmacyInventory?> GetInventory(Guid mediicationId);
    Task UpdateInventory(PharmacyInventory inventory);
    Task AddDispensedMedication(DispensedMedication dispensed);
    Task AddStockTransaction(StockTransaction transaction);

}