using ContosoPizza.Models;

namespace ContosoPizza.Services;


public interface IPizzaService
{
    Task<List<Pizza>> GetAllAsync();
    Task<Pizza?> GetAsync(int id);
    Task AddAsync(Pizza pizza);
    Task UpdateAsync(Pizza pizza);
    Task DeleteAsync(int id);

}