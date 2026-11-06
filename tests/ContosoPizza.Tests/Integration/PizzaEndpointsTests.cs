using ContosoPizza.Tests.Integration;

public class PizzaEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public PizzaEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

}