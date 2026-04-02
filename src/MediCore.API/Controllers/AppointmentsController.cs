using MediCore.Application.DTOs;
using MediCore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MediCore.API.Controllers;

[ApiController]
[Route("api/appointments")]
// [Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly AppointmentService _service;
    public AppointmentsController(AppointmentService service)
    {
        _service = service;

    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentDto dto)
    {
        var appointment = await _service.CreateAppointment(dto);
        return Ok(appointment);
    }

    [HttpGet]
    public async Task <IActionResult> GetAll()
    {
        return Ok(await _service.GetAppointments());
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetPatientAppointments(Guid patientId)
    {
        return Ok(await _service.GetPatientAppointments(patientId));
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetDoctorAppointments(string doctorId)
    {
        return Ok(await _service.GetDoctorAppointments(doctorId));
    }
}






















