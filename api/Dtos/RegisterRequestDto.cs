
using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Dtos;

public class RegisterRequestDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Username has to be at least 5 characters long!")]
    [MaxLength(30, ErrorMessage = "Username is too long!")]
    public string Username {get; set;} = string.Empty;

    [Required]
    [MinLength(8, ErrorMessage = "Password has to be at least 8 characters long!")]
    public string Password {get; set;} = string.Empty;
    
}