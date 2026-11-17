namespace ContosoPizza.Client.Services;

public interface ITokenStorage
{
    Task SaveTokenAsync(string token);
    Task<string?> ReadTokenAsync();
    Task DeleteTokenAsync();
}