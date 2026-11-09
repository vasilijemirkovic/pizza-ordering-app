using System.Net.Http.Json;
using ContosoPizza.Dtos;
using System.Net;
using System.Net.Http.Headers;

namespace ContosoPizza.Tests.Integration;

public class PizzaEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public PizzaEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreatePizza_WithoutToken_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/Pizza", CreateCapricciosaDto());

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private static PizzaRequestDto CreateCapricciosaDto()
    {
        PizzaRequestDto capricciosaDto = new PizzaRequestDto { Name = "Capricciosa", IsGlutenFree = true,
            Ingredients = {"olive oil", "tomato", "cheese", "mushrooms" } };
        return capricciosaDto;
    }

    private static PizzaRequestDto CreateMargheritaDto()
    {
        PizzaRequestDto margheritaDto = new PizzaRequestDto { Name = "Margherita", IsGlutenFree = false,
            Ingredients = {"olive oil", "tomato", "cheese" } };
        return margheritaDto;
    }
}