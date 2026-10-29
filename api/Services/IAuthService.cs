using ContosoPizza.Dtos;

namespace ContosoPizza.Services;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(RegisterRequestDto dto);
}