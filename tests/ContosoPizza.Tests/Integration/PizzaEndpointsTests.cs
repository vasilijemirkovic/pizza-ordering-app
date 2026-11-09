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

    [Fact]
    public async Task CreatePizza_TooManyIngredients_ReturnsBadRequest()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosaDto();

        capricciosaDto.Ingredients.Add("bacon");
        capricciosaDto.Ingredients.Add("egg");

        var client = _factory.CreateClient();

        var token = await TestHelpers.LoginAsync(client, "testadmin", "TestAdmin123");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("/Pizza", capricciosaDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAllPizzas_WithoutToken_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/Pizza");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreatePizza_AsAdmin_ReturnsCreated()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosaDto();

        var client = _factory.CreateClient();

        var token = await TestHelpers.LoginAsync(client, "testadmin", "TestAdmin123");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("/Pizza", capricciosaDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }


    [Fact]
    public async Task CreatePizza_AsRegularUser_ReturnsForbidden()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosaDto();

        var client = _factory.CreateClient();

        string Username = TestHelpers.CreateUniqueUsername();

        RegisterRequestDto registerRequestDto = new() { Username = Username, Password = "Password123" };

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequestDto);
        registerResponse.EnsureSuccessStatusCode();

        var token = await TestHelpers.LoginAsync(client, Username, "Password123");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("/Pizza", capricciosaDto);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetPizza_WithExistingId_ReturnsOk()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosaDto();

        var client = _factory.CreateClient();

        var token = await TestHelpers.LoginAsync(client, "testadmin", "TestAdmin123");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await client.PostAsJsonAsync("/Pizza", capricciosaDto);
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<PizzaResponseDto>();

        var response = await client.GetAsync($"/Pizza/{created!.Id}");

        var taken = await response.Content.ReadFromJsonAsync<PizzaResponseDto>();

        Assert.NotNull(taken);
        Assert.Equal("Capricciosa", taken.Name);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPizza_WithNonExistingId_ReturnsNotFound()
    {
        var client = _factory.CreateClient();

        var token = await TestHelpers.LoginAsync(client, "testadmin", "TestAdmin123");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync("/Pizza/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePizza_AsAdmin_ReturnsNoContent()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosaDto();

        var client = _factory.CreateClient();

        var token = await TestHelpers.LoginAsync(client, "testadmin", "TestAdmin123");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await client.PostAsJsonAsync("/Pizza", capricciosaDto);
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<PizzaResponseDto>();

        PizzaRequestDto margheritaDto = CreateMargheritaDto();

        var response = await client.PutAsJsonAsync($"/Pizza/{created!.Id}", margheritaDto);

        var getResponse = await client.GetAsync($"/Pizza/{created.Id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<PizzaResponseDto>();

        Assert.NotNull(updated);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal("Margherita", updated.Name);
    }

    private static PizzaRequestDto CreateCapricciosaDto()
    {
        PizzaRequestDto capricciosaDto = new PizzaRequestDto
        {
            Name = "Capricciosa",
            IsGlutenFree = true,
            Ingredients = { "olive oil", "tomato", "cheese", "mushrooms" }
        };
        return capricciosaDto;
    }

    private static PizzaRequestDto CreateMargheritaDto()
    {
        PizzaRequestDto margheritaDto = new PizzaRequestDto
        {
            Name = "Margherita",
            IsGlutenFree = false,
            Ingredients = { "olive oil", "tomato", "cheese" }
        };
        return margheritaDto;
    }
}