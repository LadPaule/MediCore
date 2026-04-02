using MediCore.Domain.Entities;
using MediCore.Application.DTOs;
using MediCore.Infrastructure.Security;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        JwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }


    //Todo: LOGIN

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var validPassword = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!validPassword)
            return Unauthorized("Invalid credentials");

        var token = await _jwtTokenService.GenerateToken(user);

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            token,
            user = new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                roles
            }
        });
    }


    //Todo: REGISTER

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(model.Email);

        if (existingUser != null)
            return BadRequest("User already exists");

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);
        await _userManager.AddToRoleAsync(user, model.Role);
        //Todo: Default role
//         var role = "User";
// 
//         if (!await _roleManager.RoleExistsAsync(role))
//         {
//             await _roleManager.CreateAsync(new IdentityRole(role));
//         }
// 
//         await _userManager.AddToRoleAsync(user, role);

        return Ok("User registered successfully");
    }


    //Todo: CURRENT USER

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User?.Identity?.Name;

        if (userId == null)
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            roles
        });
    }
}




