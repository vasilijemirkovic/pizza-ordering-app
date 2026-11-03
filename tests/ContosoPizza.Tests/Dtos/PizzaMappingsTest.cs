using ContosoPizza.Models;
using ContosoPizza.Dtos;


namespace ContosoPizza.Tests.Dtos;

public class PizzaMappingsTest
{
    [Fact]
    public void ToResponse_MapsAllProperties()
    {
        List<string> ingredients = new List<string> {"olive oil", "tomato", "cheese"};
        Pizza pizza = new Pizza {Id = 12, Name = "Margherita", IsGlutenFree = true, Ingredients = ingredients};

        PizzaResponseDto responseDto = pizza.ToResponse();
        
        Assert.Equal(12, responseDto.Id);
        Assert.Equal("Margherita", responseDto.Name);
        Assert.Equal(ingredients, responseDto.Ingredients);
        Assert.True(responseDto.IsGlutenFree);

    }
}