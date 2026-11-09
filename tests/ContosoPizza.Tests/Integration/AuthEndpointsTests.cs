using System.Net;
using System.Net.Http.Json;
using ContosoPizza.Dtos;

namespace ContosoPizza.Tests.Integration;

public class AuthEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }
}