using MediCore.Application.DTOs;
using MediCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.API.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DoctorsController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetDoctors()
    {
        var doctors = await _userManager.GetUsersInRoleAsync("Doctor");

        var result = doctors.Select(d => new
        {
            d.Id,
            d.FirstName,
            d.LastName,
            d.Email
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            return BadRequest("A user with this email already exists.");

        if (!await _roleManager.RoleExistsAsync("Doctor"))
            await _roleManager.CreateAsync(new IdentityRole("Doctor"));

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "Doctor");

        return Ok(new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email
        });
    }
}
