using ContosoPizza.Dtos;

namespace ContosoPizza.Services;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(RegisterRequestDto dto);

    Task<string?> LoginUserAsync(LoginRequestDto dto);

}