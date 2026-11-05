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

    [Fact]
    public async Task GetAllAsync_TwoPizzasAdded_ReturnsBoth()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosa();

        PizzaRequestDto margheritaDto = CreateMargherita();

        await _service.AddAsync(capricciosaDto);
        await _service.AddAsync(margheritaDto);

        List<PizzaResponseDto> pizzaResponseDtos = await _service.GetAllAsync();

        Assert.Equal(2, pizzaResponseDtos.Count);
    }


    [Fact]
    public async Task GetAsync_ExistingId_ReturnsPizza()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosa();

        PizzaResponseDto returnDto = await _service.AddAsync(capricciosaDto);

        PizzaResponseDto? pizzaResponseDto = await _service.GetAsync(returnDto.Id);

        Assert.NotNull(pizzaResponseDto);
        Assert.Equal(returnDto.Id, pizzaResponseDto.Id);
    }

            [Fact]
    public async Task GetAsync_NonExistingId_ReturnsNull()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosa();

        PizzaResponseDto returnDto = await _service.AddAsync(capricciosaDto);

        PizzaResponseDto? pizzaResponseDto = await _service.GetAsync(returnDto.Id + 1);

        Assert.Null(pizzaResponseDto);
    }

    
    [Fact]
    public async Task UpdateAsync_ExistingId_UpdatesPizza()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosa();

        PizzaResponseDto returnDto = await _service.AddAsync(capricciosaDto);

        PizzaRequestDto margheritaDto = CreateMargherita();

        bool updatedPizza = await _service.UpdateAsync(returnDto.Id, margheritaDto);

        PizzaResponseDto? pizzaResponseDto = await _service.GetAsync(returnDto.Id);

        Assert.True(updatedPizza);
        Assert.NotNull(pizzaResponseDto);
        Assert.Equal(margheritaDto.Name, pizzaResponseDto.Name);
        Assert.Equal(margheritaDto.IsGlutenFree, pizzaResponseDto.IsGlutenFree);
        Assert.Equal(margheritaDto.Ingredients, pizzaResponseDto.Ingredients);
    }

        [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsFalse()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosa();

        PizzaResponseDto returnDto = await _service.AddAsync(capricciosaDto);

        PizzaRequestDto margheritaDto = CreateMargherita();

        bool updatedPizza = await _service.UpdateAsync(returnDto.Id + 1, margheritaDto);

        Assert.False(updatedPizza);
    }

    
    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesPizza()
    {
        PizzaRequestDto capricciosaDto = CreateCapricciosa();

        PizzaResponseDto returnDto = await _service.AddAsync(capricciosaDto);

        bool deletedPizza = await _service.DeleteAsync(returnDto.Id);

        List<PizzaResponseDto> leftPizzas = await _service.GetAllAsync();

        Assert.True(deletedPizza);
        Assert.Empty(leftPizzas);
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