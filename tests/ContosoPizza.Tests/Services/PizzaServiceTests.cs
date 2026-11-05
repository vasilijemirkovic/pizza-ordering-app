using ContosoPizza.Data;
using ContosoPizza.Dtos;
using ContosoPizza.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Tests.Services;

public class PizzaServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PizzaContext _context;
    private readonly PizzaService _service;

    public PizzaServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<PizzaContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new PizzaContext(options);
        _context.Database.EnsureCreated();

        _service = new PizzaService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}