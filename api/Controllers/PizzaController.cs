using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{

    private readonly IPizzaService _service;

    public PizzaController(IPizzaService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() => _service.GetAll();


    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = _service.Get(id);

        if (pizza == null)
            return NotFound();

        return pizza;
    }


    [HttpPost]
    public IActionResult Create(Pizza pizza)
    {
        _service.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }


    [HttpPut("{id}")]
    public IActionResult Update(int id, Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();


        var existingPizza = _service.Get(id);
        if (existingPizza is null)
            return NotFound();


        _service.Update(pizza);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = _service.Get(id);

        if (pizza is null)
            return NotFound();

        _service.Delete(id);

        return NoContent();
    }

}