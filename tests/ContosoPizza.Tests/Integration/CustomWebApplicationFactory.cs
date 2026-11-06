using ContosoPizza.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ContosoPizza.Tests.Integration;

/// <summary>
/// Infrastructure, this class runs the API in test, with test database and test configurations.
/// 
/// WebApplicationFactory microservice starts the real Program.cs with all of its middlewares,
/// authentication and controllers in memory.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    /// <summary>
    /// Configures the test host: sets the environment, provides test secrets
    /// and replaces the real database with an in-memory one.
    /// </summary>
    /// <param name="builder">The web host builder used to configure the test host.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection.Open();

        // "Testing" instead of "Development", so local user secrets are not loaded
        builder.UseEnvironment("Testing");

        builder.UseSetting("Jwt:Key", "integration-test-secret-key-at-least-32-chars");
        builder.UseSetting("Admin:Username", "testadmin");
        builder.UseSetting("Admin:Password", "TestAdmin123");

        // Runs after the registrations in Program.cs, so it can replace them
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<PizzaContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<PizzaContext>>();
            services.AddDbContext<PizzaContext>(options => options.UseSqlite(_connection));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection.Dispose();
    }
}