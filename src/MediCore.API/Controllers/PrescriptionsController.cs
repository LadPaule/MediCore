using MediCore.Application.DTOs;
using MediCore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MediCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Doctor")]
public class PresriptionController : ControllerBase
{
    private readonly PrescriptionService _service;
    public PresriptionController(PrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePrescriptionDto dto)
    {
        return Ok(await _service.CreatePrescription(dto));
    }

    
}

