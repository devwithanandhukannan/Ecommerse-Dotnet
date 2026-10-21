using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Controllers;

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string Password, bool IsAdmin);

public class AuthController : ControllerBase{
    private readonly IApplicationDbContext _dbContext;
    private readonly IJwtService _jwtService;

    public AuthController(IApplicationDbContext dbContext, IJwtService jwtService){
        _dbContext = dbContext;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request){
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if(existingUser != null){
            return BadRequest("User already exists.");
        }
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User{
            Email = request.Email,
            PasswordHash = hashedPassword,
            IsAdmin = request.IsAdmin
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request){
        var user = await _dbContext.Users.FirstorDefaultAsync(u => u.Email == request.Email);
        if(user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)){
            return Unauthorized("Invalid email or password.");
        }
        string token = _jwtService.GenerateToken(user);
        return Ok(new { Token = token });
    }
}