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

        [Fact]
    public async Task AddAsync_ValidPizza_ReturnsPizzaWithId()
    {
        PizzaRequestDto requestDto = CreateCapricciosa();

        PizzaResponseDto responseDto = await _service.AddAsync(requestDto);

        Assert.True(responseDto.Id > 0);
        Assert.Equal("Capricciosa", responseDto.Name);
        Assert.True(responseDto.IsGlutenFree);
        Assert.Equal(requestDto.Ingredients, responseDto.Ingredients);
    }

    private static PizzaRequestDto CreateCapricciosa()
    {
        return new PizzaRequestDto
        {
            Name = "Capricciosa",
            IsGlutenFree = true,
            Ingredients = ["olive oil", "tomato", "cheese", "mushrooms"]
        };
    }

    private static PizzaRequestDto CreateMargherita()
    {
        return new PizzaRequestDto
        {
            Name = "Margherita",
            IsGlutenFree = false,
            Ingredients = ["olive oil", "tomato", "cheese"]
        };
    }
}