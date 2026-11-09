using System.Net.Http.Json;
using ContosoPizza.Dtos;

namespace ContosoPizza.Tests.Integration;

public static class TestHelpers
{
    private record LoginResponse(string Token);

    internal static async Task<string> LoginAsync(HttpClient client, string username, string password)
    {
        var loginDto = new LoginRequestDto { Username = username, Password = password };

        var response = await client.PostAsJsonAsync("/Auth/login", loginDto);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result!.Token;
    }
}