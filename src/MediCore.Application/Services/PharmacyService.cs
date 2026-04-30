using MediCore.Application.DTOs;
using MediCore.Application.Interfaces;
using MediCore.Domain.Entities;

namespace MediCore.Application.Services;

public class PharmacyService
{
    private readonly IPharmacyRepository  _repository;

    public PharmacyService(IPharmacyRepository repository)
    {
        _repository = repository;
    }
    public async Task<string> DispenseMedication(DispenseMedicationDto dto)
    {
        // Todo: handle inventory reduction logic
        var inventory = await _repository.GetInventory(dto.MedicationId);

        if (inventory == null)
            return "No Medication in the Inventory";
        if(inventory.QuantityInStock < dto.Quantity)
            return "There's not enough stock";
        inventory.QuantityInStock -= dto.Quantity;
        await _repository.UpdateInventory(inventory);

        var dispensed = new DispensedMedication
        {
            PrescriptionId = dto.PrescriptionId,
            MedicationId = dto.MedicationId,
            Quantity = dto.Quantity
        };

        await _repository.AddDispensedMedication(dispensed);

        var transaction = new StockTransaction
        {
            MedicationId = dto.MedicationId,
            QuantityChanged = dto.Quantity,
            TransactionType = "Dispense"
        };

        await _repository.AddStockTransaction(transaction);

        return "Medication dispensed Successfully";
    }
}