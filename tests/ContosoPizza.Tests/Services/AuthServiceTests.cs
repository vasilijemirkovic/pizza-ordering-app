
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ContosoPizza.Data;
using ContosoPizza.Shared.Dtos;
using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ContosoPizza.Tests.Services;

public class AuthServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PizzaContext _context;
    private readonly AuthService _service;

    public AuthServiceTests()
    {

        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<PizzaContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new PizzaContext(options);
        _context.Database.EnsureCreated();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test-secret-key-that-is-at-least-32-characters",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:ExpiresInMinutes"] = "60"
            })
            .Build();

        _service = new AuthService(_context, new PasswordHasher<User>(), configuration);
    }
    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task RegisterUserAsync_NewUsername_ReturnsTrueAndSavesUser()
    {
        RegisterRequestDto dto = new RegisterRequestDto { Username = "TestName", Password = "TestPassword" };

        bool registerResult = await _service.RegisterUserAsync(dto);

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(eachUser => eachUser.Username == dto.Username);

        Assert.True(registerResult);
        Assert.NotNull(user);
        Assert.Equal("User", user.Role);
        Assert.NotEqual(dto.Password, user.PasswordHash);
    }

    [Fact]
    public async Task RegisterUserAsync_ExistingUsername_ReturnsFalse()
    {
        RegisterRequestDto dto = new RegisterRequestDto { Username = "TestName", Password = "TestPassword" };

        await _service.RegisterUserAsync(dto);

        bool registerResult = await _service.RegisterUserAsync(dto);

        var userCount = await _context.Users
            .CountAsync(eachUser => eachUser.Username == dto.Username);

        Assert.False(registerResult);
        Assert.Equal(1, userCount);
    }

    [Fact]
    public async Task LoginUserAsync_ValidCredentials_ReturnsToken()
    {
        RegisterRequestDto registerRequestDto = new RegisterRequestDto { Username = "TestName", Password = "TestPassword" };
        await _service.RegisterUserAsync(registerRequestDto);

        LoginRequestDto loginRequestDto = new LoginRequestDto { Username = "TestName", Password = "TestPassword" };
        string? resultToken = await _service.LoginUserAsync(loginRequestDto);

        var token = new JwtSecurityTokenHandler().ReadJwtToken(resultToken);

        Assert.False(string.IsNullOrEmpty(resultToken));
        Assert.Equal("User", token.Claims.FirstOrDefault(eachClaim => eachClaim.Type == ClaimTypes.Role)?.Value);
        Assert.Equal("TestName", token.Claims.FirstOrDefault(eachClaim => eachClaim.Type == ClaimTypes.Name)?.Value);
    }

    [Fact]
    public async Task LoginUserAsync_WrongUsername_ReturnsNull()
    {
        RegisterRequestDto registerRequestDto = new RegisterRequestDto { Username = "TestName", Password = "TestPassword" };
        await _service.RegisterUserAsync(registerRequestDto);

        LoginRequestDto loginRequestDto = new LoginRequestDto { Username = "TestName1", Password = "TestPassword" };
        string? resultToken = await _service.LoginUserAsync(loginRequestDto);

        Assert.Null(resultToken);
    }

    [Fact]
    public async Task LoginUserAsync_WrongPassword_ReturnsNull()
    {
        RegisterRequestDto registerRequestDto = new RegisterRequestDto { Username = "TestName", Password = "TestPassword" };
        await _service.RegisterUserAsync(registerRequestDto);

        LoginRequestDto loginRequestDto = new LoginRequestDto { Username = "TestName", Password = "TestPassword1" };
        string? resultToken = await _service.LoginUserAsync(loginRequestDto);

        Assert.Null(resultToken);
    }

    [Fact]
    public async Task LoginUserAsync_UserNotExists_ReturnsNull()
    {
        LoginRequestDto loginRequestDto = new LoginRequestDto { Username = "TestName", Password = "TestPassword" };
        string? resultToken = await _service.LoginUserAsync(loginRequestDto);

        Assert.Null(resultToken);
    }
}