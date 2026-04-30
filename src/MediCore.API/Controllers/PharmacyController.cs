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

    [HttpPost("medicines")]
    public async Task<ActionResult<MedicineDto>> CreateMedicine([FromBody] CreateMedicineDto dto)
    {
        var medicine = new Medication
        {
            Name = dto.Name,
            StockQuantity = dto.StockQuantity,
            ReorderLevel = dto.ReorderLevel,
            Price = dto.Price,
            ExpiryDate = DateTime.SpecifyKind(dto.ExpiryDate, DateTimeKind.Utc),
            Description = dto.Description,
            Manufacturer = dto.Manufacturer
        };

        _context.Medications.Add(medicine);

        _context.StockTransactions.Add(new StockTransaction
        {
            MedicationId = medicine.Id,
            QuantityChanged = dto.StockQuantity,
            TransactionType = "InitialStock",
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return Ok(new MedicineDto
        {
            Id = medicine.Id,
            Name = medicine.Name,
            StockQuantity = medicine.StockQuantity,
            Price = medicine.Price,
            ExpiryDate = medicine.ExpiryDate
        });
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
            PrescriptionId = dto.PrescriptionId != Guid.Empty ? dto.PrescriptionId : null,
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

    [HttpDelete("medicines/{id:guid}")]
    public async Task<IActionResult> DeleteMedicine(Guid id)
    {
        var medicine = await _context.Medications.FindAsync(id);
        if (medicine == null)
            return NotFound();

        _context.Medications.Remove(medicine);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("medicines/{id:guid}")]
    public async Task<ActionResult<MedicineDetailDto>> GetMedicine(Guid id)
    {
        var medicine = await _context.Medications.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (medicine == null)
            return NotFound();

        var transactions = await _context.StockTransactions
            .AsNoTracking()
            .Where(x => x.MedicationId == id)
            .OrderByDescending(x => x.CreatedAt)
            .Take(20)
            .Select(x => new StockTransactionDto
            {
                Id = x.Id,
                QuantityChanged = x.QuantityChanged,
                TransactionType = x.TransactionType,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();

        return Ok(new MedicineDetailDto
        {
            Id = medicine.Id,
            Name = medicine.Name,
            StockQuantity = medicine.StockQuantity,
            ReorderLevel = medicine.ReorderLevel,
            Price = medicine.Price,
            ExpiryDate = medicine.ExpiryDate,
            Description = medicine.Description,
            Manufacturer = medicine.Manufacturer,
            CreatedAt = medicine.CreatedAt,
            RecentTransactions = transactions
        });
    }

    [HttpPost("medicines/{id:guid}/restock")]
    public async Task<IActionResult> Restock(Guid id, [FromBody] RestockMedicineDto dto)
    {
        if (dto.Quantity <= 0)
            return BadRequest("Quantity must be greater than zero.");

        var medicine = await _context.Medications.FirstOrDefaultAsync(x => x.Id == id);
        if (medicine == null)
            return NotFound("Medicine not found.");

        medicine.StockQuantity += dto.Quantity;

        _context.StockTransactions.Add(new StockTransaction
        {
            MedicationId = medicine.Id,
            QuantityChanged = dto.Quantity,
            TransactionType = "Restock",
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return Ok(new { message = $"Added {dto.Quantity} units. New stock: {medicine.StockQuantity}.", stockQuantity = medicine.StockQuantity });
    }

    [HttpPost("medicines/{id:guid}/adjust")]
    public async Task<IActionResult> AdjustStock(Guid id, [FromBody] AdjustStockDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Reason))
            return BadRequest("Reason is required for stock adjustments.");

        var medicine = await _context.Medications.FirstOrDefaultAsync(x => x.Id == id);
        if (medicine == null)
            return NotFound("Medicine not found.");

        var delta = dto.NewQuantity - medicine.StockQuantity;
        medicine.StockQuantity = dto.NewQuantity;

        _context.StockTransactions.Add(new StockTransaction
        {
            MedicationId = medicine.Id,
            QuantityChanged = delta,
            TransactionType = $"Adjustment: {dto.Reason}",
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return Ok(new { message = $"Stock adjusted to {medicine.StockQuantity}.", stockQuantity = medicine.StockQuantity });
    }

    [HttpPut("medicines/{id:guid}")]
    public async Task<IActionResult> UpdateMedicine(Guid id, [FromBody] UpdateMedicineDto dto)
    {
        var medicine = await _context.Medications.FirstOrDefaultAsync(x => x.Id == id);
        if (medicine == null)
            return NotFound();

        medicine.Name = dto.Name;
        medicine.ReorderLevel = dto.ReorderLevel;
        medicine.Price = dto.Price;
        medicine.ExpiryDate = DateTime.SpecifyKind(dto.ExpiryDate, DateTimeKind.Utc);
        medicine.Description = dto.Description;
        medicine.Manufacturer = dto.Manufacturer;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Medicine updated." });
    }
}


