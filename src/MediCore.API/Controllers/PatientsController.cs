using MediCore.Application.DTOs;
using MediCore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.API.Controllers;

// [ApiController]
[Route("api/[controller]")]
// [Authorize]
public class PatientsController : ControllerBase
{
    private readonly PatientService _patientService;

    public PatientsController(PatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatient(CreatePatientDto dto)
    {
        var patient = await _patientService.CreatePatient(dto);
        return Ok(patient);
    }

    [HttpGet]
    public async Task<IActionResult> GetPatients()
    {
        var patients = await _patientService.GetPatients();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(Guid id)
    {
        var patient = await _patientService.GetPatient(id);

        if (patient == null)
            return NotFound();

        return Ok(patient);
    }
}