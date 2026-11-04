
using ContosoPizza.Data;
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

    public AuthServiceTests(){

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
}