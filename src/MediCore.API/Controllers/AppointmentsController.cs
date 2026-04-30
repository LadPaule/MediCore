using System.Security.Claims;
using MediCore.Application.DTOs;
using MediCore.Application.Services;
using MediCore.Domain.Entities;
using MediCore.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MediCore.API.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly AppointmentService _service;
    private readonly PatientService _patientService;
    private readonly AppDbContext _context;

    public AppointmentsController(AppointmentService service, PatientService patientService, AppDbContext context)
    {
        _service = service;
        _patientService = patientService;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentDto dto)
    {
        var appointment = await _service.CreateAppointment(dto);

        // If the patient has no doctor assigned yet, the doctor on this
        // appointment becomes their primary doctor. Booking implies assignment.
        var patient = await _patientService.GetPatient(dto.PatientId);
        if (patient != null && string.IsNullOrEmpty(patient.AssignedDoctorId))
        {
            await _patientService.AssignDoctor(dto.PatientId, dto.DoctorId);
        }

        return Ok(appointment);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? doctorId = null)
    {
        var query = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(doctorId))
            query = query.Where(a => a.DoctorId == doctorId);

        var appointments = await query
            .OrderByDescending(a => a.AppointmentDate)
            .Select(a => new
            {
                a.Id,
                a.PatientId,
                PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                a.DoctorId,
                DoctorName = a.Doctor != null ? "Dr. " + a.Doctor.FirstName + " " + a.Doctor.LastName : "N/A",
                a.AppointmentDate,
                a.Reason,
                a.Status
            })
            .ToListAsync();

        return Ok(appointments);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMine()
    {
        var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(doctorId))
            return Unauthorized();

        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.DoctorId == doctorId)
            .OrderByDescending(a => a.AppointmentDate)
            .Select(a => new
            {
                a.Id,
                a.PatientId,
                PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                a.DoctorId,
                DoctorName = a.Doctor != null ? "Dr. " + a.Doctor.FirstName + " " + a.Doctor.LastName : "N/A",
                a.AppointmentDate,
                a.Reason,
                a.Status
            })
            .ToListAsync();

        return Ok(appointments);
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetPatientAppointments(Guid patientId)
    {
        var appointments = await _context.Appointments
            .Include(a => a.Doctor)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .Select(a => new
            {
                a.Id,
                a.PatientId,
                a.DoctorId,
                DoctorName = a.Doctor != null ? "Dr. " + a.Doctor.FirstName + " " + a.Doctor.LastName : "N/A",
                a.AppointmentDate,
                a.Reason,
                a.Status
            })
            .ToListAsync();

        return Ok(appointments);
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetDoctorAppointments(string doctorId)
    {
        return Ok(await _service.GetDoctorAppointments(doctorId));
    }
}
