using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Dtos;

/// <summary>
/// Represents the DTO sent by the client when creating or editing a pizza.
/// </summary>

public class PizzaRequestDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public bool IsGlutenFree { get; set; }

    [MinLength(1, ErrorMessage = "Pizza must contain al least 1 ingredient!")]
    [MaxLength(5, ErrorMessage = "Pizza can contain at most 5 ingredients!")]
    public List<string> Ingredients { get; set; } = new();    
}