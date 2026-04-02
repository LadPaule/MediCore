using MediCore.Application.DTOs;
using MediCore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MediCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles ="Doctor")]
public class MedicalRecordsController : ControllerBase
{
    private readonly MedicalRecordService _service;
    public MedicalRecordsController(MedicalRecordService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMedicalRecordDto dto)
    {
        return Ok(await _service.CreateRecord(dto));
    }
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetActionResultAsync(Guid patientId)
    {
        return Ok(await _service.GetPatientRecords(patientId));
    }
}