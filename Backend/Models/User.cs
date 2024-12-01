using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class User
    {
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // Храните хэш пароля, а не сам пароль
    public string Role { get; set; } = "User"; // Например: "Admin", "User"
    }
}