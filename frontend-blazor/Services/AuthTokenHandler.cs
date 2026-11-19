using System.Net.Http.Headers;

namespace ContosoPizza.Client.Services;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly ITokenStorage _tokenStorage;

    public AuthTokenHandler(ITokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenStorage.ReadTokenAsync();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}