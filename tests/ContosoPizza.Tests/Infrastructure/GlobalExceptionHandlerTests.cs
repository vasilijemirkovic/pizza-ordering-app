using ContosoPizza.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace ContosoPizza.Tests.Infrastructure;

public class GlobalExceptionHandlerTests
{
    private readonly GlobalExceptionHandler _handler;
    private readonly HttpContext _httpContext;

    public GlobalExceptionHandlerTests()
    {
        _handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);
        _httpContext = new DefaultHttpContext();

        // Replace the default response body so the test can read what the handler writes
        _httpContext.Response.Body = new MemoryStream();
    }

    [Fact]
    public async Task TryHandleAsync_AnyException_Returns500WithoutDetails()
    {
        const string secretMessage = "Secret internal details";
        Exception exception = new Exception(secretMessage);

        bool handled = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

        // Rewind the stream to the beginning before reading what the handler wrote
        _httpContext.Response.Body.Position = 0;
        string body = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);
        Assert.Contains("An unexpected error occurred.", body);
        Assert.DoesNotContain(secretMessage, body);
    }
}