using System.Security.Claims;
using MediCore.Application.DTOs;
using MediCore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        return Ok(Project(patient));
    }

    [HttpGet]
    public async Task<IActionResult> GetPatients([FromQuery] string? doctorId = null)
    {
        var patients = string.IsNullOrWhiteSpace(doctorId)
            ? await _patientService.GetPatients()
            : await _patientService.GetPatientsByDoctor(doctorId);

        return Ok(patients.Select(Project));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyPatients()
    {
        var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(doctorId))
            return Unauthorized();

        var patients = await _patientService.GetPatientsByDoctor(doctorId);
        return Ok(patients.Select(Project));
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetByDoctor(string doctorId)
    {
        var patients = await _patientService.GetPatientsByDoctor(doctorId);
        return Ok(patients.Select(Project));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(Guid id)
    {
        var patient = await _patientService.GetPatient(id);

        if (patient == null)
            return NotFound();

        return Ok(Project(patient));
    }

    [HttpPut("{id}/assign-doctor")]
    public async Task<IActionResult> AssignDoctor(Guid id, [FromBody] AssignDoctorDto dto)
    {
        var patient = await _patientService.AssignDoctor(id, dto.DoctorId);
        if (patient == null)
            return NotFound();

        return Ok(Project(patient));
    }

    private static object Project(MediCore.Domain.Entities.Patient p) => new
    {
        p.Id,
        p.FirstName,
        p.LastName,
        p.DateOfBirth,
        p.Gender,
        p.PhoneNumber,
        p.Email,
        p.Address,
        p.AssignedDoctorId,
        AssignedDoctorName = p.AssignedDoctor != null
            ? $"Dr. {p.AssignedDoctor.FirstName} {p.AssignedDoctor.LastName}"
            : null
    };
}
