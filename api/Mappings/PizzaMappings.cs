using ContosoPizza.Models;
using ContosoPizza.Shared.Dtos;

namespace ContosoPizza.Mappings;


public static class PizzaMappings
{
    public static PizzaResponseDto ToResponse(this Pizza pizza) => new()
    {
        Id = pizza.Id,
        Name = pizza.Name ?? string.Empty,
        IsGlutenFree = pizza.IsGlutenFree,
        Ingredients = pizza.Ingredients
    };

    public static Pizza ToEntity(this PizzaRequestDto dto) => new()
    {
        Name = dto.Name,
        IsGlutenFree = dto.IsGlutenFree,
        Ingredients = dto.Ingredients
    };
}