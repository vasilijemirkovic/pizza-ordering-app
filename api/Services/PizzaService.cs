using ContosoPizza.Data;
using ContosoPizza.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly PizzaContext _context;
    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    public async Task<List<PizzaResponseDto>> GetAllAsync() =>
        await _context.Pizzas
        .AsNoTracking()
        .Select(p => p.ToResponse())
        .ToListAsync();
    
    public async Task<PizzaResponseDto?> GetAsync(int id)
    {
        var pizza = await _context.Pizzas
        .AsNoTracking()
        .FirstOrDefaultAsync(eachPizza => eachPizza.Id == id);

        return pizza?.ToResponse();
    }

    public async Task<PizzaResponseDto> AddAsync(PizzaRequestDto dto)
    {
        var pizza = dto.ToEntity();
        _context.Pizzas.Add(pizza);
        await _context.SaveChangesAsync();

        return pizza.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pizza = await _context.Pizzas.FindAsync(id);

        if (pizza is null)
            return false;

        _context.Pizzas.Remove(pizza);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, PizzaRequestDto dto)
    {
        var pizza = await _context.Pizzas.FindAsync(id);
        if(pizza is null)
            return false;
        
        pizza.Name = dto.Name;
        pizza.IsGlutenFree = dto.IsGlutenFree;
        pizza.Ingredients = dto.Ingredients;
        
        await _context.SaveChangesAsync();
        return true;
    }
}