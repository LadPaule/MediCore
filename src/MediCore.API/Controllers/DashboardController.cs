using MediCore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;
    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("patients")]
    public async Task<int> GetPatientCount()
    {
        return await _context.Patients.CountAsync();
    }
    [HttpGet("appointments/today")]
    public async Task<int> GetTodayAppointments()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Appointments
            .Where(a => a.AppointmentDate.Date == today)
            .CountAsync();
    }
}