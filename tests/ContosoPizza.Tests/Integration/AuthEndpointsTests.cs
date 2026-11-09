using System.Net;
using System.Net.Http.Json;
using ContosoPizza.Dtos;

namespace ContosoPizza.Tests.Integration;

public class AuthEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Register_ValidData_ReturnsCreated()
    {
        var client = _factory.CreateClient();

        string username = TestHelpers.CreateUniqueUsername();

        RegisterRequestDto registerRequestDto = new() { Username = username, Password = "Password123" };

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequestDto);

        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);
    }


    [Fact]
    public async Task Register_ShortPassword_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        string username = TestHelpers.CreateUniqueUsername();

        RegisterRequestDto registerRequestDto = new() { Username = username, Password = "test123" };

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequestDto);

        Assert.Equal(HttpStatusCode.BadRequest, registerResponse.StatusCode);
    }
}