using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class Pizza
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public bool IsGlutenFree { get; set; }

    [MaxLength(5, ErrorMessage = "Pizza can contain at most 5 ingredients!")]
    public List<string> Ingredients { get; set; } = new();

}