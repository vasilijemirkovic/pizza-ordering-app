using System.Net.Http.Json;
using ContosoPizza.Dtos;
using System.Net;
using System.Net.Http.Headers;

namespace ContosoPizza.Tests.Integration;

public class PizzaEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public PizzaEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }
}