using ContosoPizza.Models;

namespace ContosoPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly List<Pizza> _pizzas;
    private int nextId = 3;
    public PizzaService()
    {
        _pizzas = new List<Pizza>
        {
            new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false,
            Ingredients = new() {"tomato sauce", "mozzarella", "basil", "olive oil"} },
            new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true,
            Ingredients = new() {"tomato sauce", "paprika", "mushroom", "olive", "olive oil"} }
        };
    }

    public List<Pizza> GetAll() => _pizzas;

    public Pizza? Get(int id) => _pizzas.FirstOrDefault(p => p.Id == id);

    public void Add(Pizza pizza)
    {
        pizza.Id = nextId++;
        _pizzas.Add(pizza);
    }

    public void Delete(int id)
    {
        var pizza = Get(id);
        if (pizza is null)
            return;

        _pizzas.Remove(pizza);
    }

    public void Update(Pizza pizza)
    {
        var index = _pizzas.FindIndex(p => p.Id == pizza.Id);
        if (index == -1)
            return;

        _pizzas[index] = pizza;
    }
}