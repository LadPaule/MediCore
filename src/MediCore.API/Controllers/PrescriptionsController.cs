using MediCore.Application.DTOs;
using MediCore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MediCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Doctor")]
public class PrescriptionsController : ControllerBase
{
    private readonly PrescriptionService _service;
    public PrescriptionsController(PrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePrescriptionDto dto)
    {
        return Ok(await _service.CreatePrescription(dto));
    }

    
}

