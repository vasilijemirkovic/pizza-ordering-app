using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace ContosoPizza.Client.Services;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{

    private readonly ITokenStorage _tokenStorage;

    public JwtAuthenticationStateProvider(ITokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenStorage.ReadTokenAsync();

        if (string.IsNullOrEmpty(token))
        {
            return Anonymous();
        }

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        if (jwt.ValidTo < DateTime.UtcNow)
        {
            await _tokenStorage.DeleteTokenAsync();
            return Anonymous();
        }

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyUserChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static AuthenticationState Anonymous()
    {
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}