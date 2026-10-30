using ContosoPizza.Dtos;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterUser(RegisterRequestDto dto)
    {
        var registered = await _service.RegisterUserAsync(dto);
        return registered ? StatusCode(201) : Conflict("Username is already taken.");
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginUser(LoginRequestDto dto)
    {
        var token = await _service.LoginUserAsync(dto);
        return token != null ? Ok(new { token }) : Unauthorized("Invalid username or password.");
    }
}

