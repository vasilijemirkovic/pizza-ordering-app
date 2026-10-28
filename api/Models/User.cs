using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Models;



[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? PasswordHash { get; set; }
    public string? Role { get; set; }
}