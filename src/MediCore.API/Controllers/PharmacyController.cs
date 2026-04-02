using MediCore.Application.DTOs;
using MediCore.Infrastructure.Data;
using MediCore.Application.DTOs.Pharmacy;
using MediCore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.API.Controllers;


[ApiController]
[Route("api/pharmacy")]
public class PharmacyController : ControllerBase
{
    private readonly AppDbContext _context;

    public PharmacyController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<PharmacyStatsDto>> GetStats()
    {
        var medicines = await _context.Medications.AsNoTracking().ToListAsync();
        var cutoff = DateTime.UtcNow.Date.AddDays(30);

        var result = new PharmacyStatsDto
        {
            TotalMedicines = medicines.Count,
            AvailableStock = medicines.Sum(x => x.StockQuantity),
            LowStock = medicines.Count(x => x.StockQuantity <= x.ReorderLevel),
            ExpiringSoon = medicines.Count(x => x.ExpiryDate.Date <= cutoff)
        };

        return Ok(result);
    }

    [HttpGet("medicines")]
    public async Task<ActionResult<List<MedicineDto>>> GetMedicines()
    {
        var medicines = await _context.Medications
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new MedicineDto
            {
                Id = x.Id,
                Name = x.Name,
                StockQuantity = x.StockQuantity,
                Price = x.Price,
                ExpiryDate = x.ExpiryDate
            })
            .ToListAsync();

        return Ok(medicines);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<List<MedicineDto>>> GetLowStock()
    {
        var medicines = await _context.Medications
            .AsNoTracking()
            .Where(x => x.StockQuantity <= x.ReorderLevel)
            .OrderBy(x => x.StockQuantity)
            .Select(x => new MedicineDto
            {
                Id = x.Id,
                Name = x.Name,
                StockQuantity = x.StockQuantity,
                Price = x.Price,
                ExpiryDate = x.ExpiryDate
            })
            .ToListAsync();

        return Ok(medicines);
    }

    [HttpGet("expiring")]
    public async Task<ActionResult<List<MedicineDto>>> GetExpiring()
    {
        var cutoff = DateTime.UtcNow.Date.AddDays(30);

        var medicines = await _context.Medications
            .AsNoTracking()
            .Where(x => x.ExpiryDate.Date <= cutoff)
            .OrderBy(x => x.ExpiryDate)
            .Select(x => new MedicineDto
            {
                Id = x.Id,
                Name = x.Name,
                StockQuantity = x.StockQuantity,
                Price = x.Price,
                ExpiryDate = x.ExpiryDate
            })
            .ToListAsync();

        return Ok(medicines);
    }

    [HttpPost("dispense")]
    public async Task<IActionResult> Dispense([FromBody] DispenseMedicationDto dto)
    {
        if (dto.Quantity <= 0)
            return BadRequest("Quantity must be greater than zero.");

        var medicine = await _context.Medications.FirstOrDefaultAsync(x => x.Id == dto.MedicationId);
        if (medicine == null)
            return NotFound("Medicine not found.");

        if (medicine.StockQuantity < dto.Quantity)
            return BadRequest("Not enough stock.");

        medicine.StockQuantity -= dto.Quantity;

        _context.DispensedMedications.Add(new DispensedMedication
        {
            
            MedicationId = medicine.Id,
            Quantity = dto.Quantity,
            DispensedAt = DateTime.UtcNow
        });

        _context.StockTransactions.Add(new StockTransaction
        {
            MedicationId = medicine.Id,
            QuantityChanged = -dto.Quantity,
            TransactionType = "Dispense",
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return Ok(new { message = "Medication dispensed successfully." });
    }

    [HttpDelete("medicines/{id:int}")]
    public async Task<IActionResult> DeleteMedicine(int id)
    {
        var medicine = await _context.Medications.FindAsync(id);
        if (medicine == null)
            return NotFound();

        _context.Medications.Remove(medicine);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}


