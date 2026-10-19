using ContosoPizza.Dtos;

namespace ContosoPizza.Services;

public interface IPizzaService
{
    Task<List<PizzaResponseDto>> GetAllAsync();
    Task<PizzaResponseDto?> GetAsync(int id);
    Task<PizzaResponseDto> AddAsync(PizzaRequestDto dto);
    Task<bool> UpdateAsync(int id, PizzaRequestDto dto);
    Task<bool> DeleteAsync(int id);
}