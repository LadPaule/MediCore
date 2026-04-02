using MediCore.Application.Services.Analytics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.API.Controllers;

[ApiController]
[Route("/api/analytics")]
// [Authorize(Roles ="Admin")]

public class AnalyticsController : ControllerBase
{
    private readonly HospitalAnalyticsService _service;
    public AnalyticsController(HospitalAnalyticsService service)
    {
        _service = service;

    }
    [HttpGet("patient-count")]
    public async Task<IActionResult> GetPatientCount()
    {
        return Ok(await _service.GetPatientCount());
    }

    [HttpGet("prescription-count")]
    public async Task<IActionResult> GetPrescriptionCount()
    {
        return Ok(await _service.GetPrescriptionCount());

    }

    [HttpGet("top-medications")]
    public async Task<IActionResult> GetTopMedications()
    {
        return Ok(await _service.GetTopMedications());
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        return Ok(await _service.GetLowStockMedications());
    }
}



