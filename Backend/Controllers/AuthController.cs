using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        if (_context.Users.Any(u => u.Username == user.Username))
            return BadRequest("Такой пользователь уже есть");
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> login([FromBody] User user)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
        if(existingUser == null || !BCrypt.Net.BCrypt.Verify(user.PasswordHash, existingUser.PasswordHash))
        return Unauthorized("Invalid credentials");
        var token = GenerateJwtToken(existingUser);
        return Ok(new { Token = token});
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Key"]));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
            }
}