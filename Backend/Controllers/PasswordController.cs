using System;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Identity;


[ApiController]
[Route("api/[controller]")]
public class PasswordController : ControllerBase
{
    [HttpGet("hash")]
    public IActionResult GetHashedPassword(string password)
    {
        var passwordHasher = new PasswordHasher<string>();
        string hashedPassword = passwordHasher.HashPassword(null, password);
        return Ok(new { HashedPassword = hashedPassword });
    }
}
