using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediCore.Domain.Entities;
using MediCore.Application.DTOs;

[ApiController]
[Route("api/admin/users")]
public class AdminUsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminUsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET all users
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userManager.Users
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.UserName,
                u.CreatedAt
            })
            .ToList();

        return Ok(users);
    }

    //Todo CREATE user
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (!string.IsNullOrEmpty(dto.Role))
        {
            await _userManager.AddToRoleAsync(user, dto.Role);
        }

        return Ok(user);
    }

    //Todo UPDATE user
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.UserName = dto.Email;

        await _userManager.UpdateAsync(user);

        return Ok(user);
    }

    // Todo: DELETE user
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        await _userManager.DeleteAsync(user);

        return Ok();
    }


    // Todo: Get User roles
    [HttpGet("roles")]
    public IActionResult GetRoles()
    {
        return Ok(_roleManager.Roles.Select(r => r.Name));
    }

    // Todo: Assign Roles

    [HttpPost("{id}/role")]
    public async Task<IActionResult> AssignRole(string id, AssignRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        await _userManager.AddToRoleAsync(user, dto.Role);

        return Ok();
    }
    
}






