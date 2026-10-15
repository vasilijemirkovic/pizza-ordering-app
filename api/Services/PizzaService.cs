using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly PizzaContext _context;
    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    public async Task<List<Pizza>> GetAllAsync() => 
        await _context.Pizzas.AsNoTracking().ToListAsync();
    
    public async Task<Pizza?> GetAsync(int id) => 
        await _context.Pizzas.AsNoTracking().FirstOrDefaultAsync(eachPizza => eachPizza.Id == id);

    public async Task AddAsync(Pizza pizza)
    {
        _context.Pizzas.Add(pizza);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var pizza = await _context.Pizzas.FindAsync(id);

        if (pizza is null)
            return;

        _context.Pizzas.Remove(pizza);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pizza pizza)
    {
        _context.Pizzas.Update(pizza);
        await _context.SaveChangesAsync();
    }
}