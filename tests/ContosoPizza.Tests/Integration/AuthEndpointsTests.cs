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

    [Fact]
    public async Task Register_ExistingUsername_ReturnsConflict()
    {
        var client = _factory.CreateClient();

        string username = TestHelpers.CreateUniqueUsername();

        RegisterRequestDto registerRequestDto = new() { Username = username, Password = "Password123" };

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequestDto);
        registerResponse.EnsureSuccessStatusCode();

        var anotherRegisterResponse = await client.PostAsJsonAsync("/Auth/register", registerRequestDto);

        Assert.Equal(HttpStatusCode.Conflict, anotherRegisterResponse.StatusCode);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        var client = _factory.CreateClient();

        string username = TestHelpers.CreateUniqueUsername();

        RegisterRequestDto registerRequestDto = new() { Username = username, Password = "Password123" };

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequestDto);
        registerResponse.EnsureSuccessStatusCode();

        LoginRequestDto loginRequestDto = new() { Username = username, Password = "Password123" };

        var loginResponse = await client.PostAsJsonAsync("/Auth/login", loginRequestDto);

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();

        string username = TestHelpers.CreateUniqueUsername();
        string password = "Password123";

        RegisterRequestDto registerRequestDto = new() { Username = username, Password = password };

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequestDto);
        registerResponse.EnsureSuccessStatusCode();

        LoginRequestDto loginRequestDto = new() { Username = username, Password = "Password" };

        var loginResponse = await client.PostAsJsonAsync("/Auth/login", loginRequestDto);

        Assert.Equal(HttpStatusCode.Unauthorized, loginResponse.StatusCode);
    }
}