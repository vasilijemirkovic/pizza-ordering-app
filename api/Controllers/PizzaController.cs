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
    public async Task<ActionResult<List<Pizza>>> GetAll() => 
        await _service.GetAllAsync();


    [HttpGet("{id}")]
    public async Task<ActionResult<Pizza>> Get(int id)
    {
        var pizza = await _service.GetAsync(id);

        if (pizza is null)
            return NotFound();

        return pizza;
    }


    [HttpPost]
    public async Task<IActionResult> Create(Pizza pizza)
    {
        await _service.AddAsync(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();


        var existingPizza = await _service.GetAsync(id);
        if (existingPizza is null)
            return NotFound();


        await _service.UpdateAsync(pizza);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pizza = await _service.GetAsync(id);

        if (pizza is null)
            return NotFound();

        await _service.DeleteAsync(id);

        return NoContent();
    }

}