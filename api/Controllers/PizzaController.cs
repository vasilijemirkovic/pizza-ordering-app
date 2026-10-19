using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;
using ContosoPizza.Dtos;

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
    public async Task<ActionResult<List<PizzaResponseDto>>> GetAll() => 
        await _service.GetAllAsync();


    [HttpGet("{id}")]
    public async Task<ActionResult<PizzaResponseDto>> Get(int id)
    {
        var pizza = await _service.GetAsync(id);

        if (pizza is null)
            return NotFound();

        return pizza;
    }


    [HttpPost]
    public async Task<ActionResult<PizzaResponseDto>> Create(PizzaRequestDto dto)
    {
        var created = await _service.AddAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PizzaRequestDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted? NoContent() : NotFound();
    }
}