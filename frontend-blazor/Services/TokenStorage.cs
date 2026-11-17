using Microsoft.JSInterop;

namespace ContosoPizza.Client.Services;

public class TokenStorage : ITokenStorage
{
    private readonly IJSRuntime _jsRuntime;

    private const string TokenKey = "auth_token";

    public TokenStorage(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveTokenAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
    }

    public async Task<string?> ReadTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
    }

    public async Task DeleteTokenAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    }
}