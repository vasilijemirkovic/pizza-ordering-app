namespace ContosoPizza.Dtos;

/// <summary>
/// Represents the DTO retured to the client.
/// </summary>
public class PizzaResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsGlutenFree { get; set; }
    public List<string> Ingredients { get; set; } = new();
}