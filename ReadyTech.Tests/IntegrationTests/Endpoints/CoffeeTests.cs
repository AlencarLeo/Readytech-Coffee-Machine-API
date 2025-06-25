using System.Net;
using IntegrationTests.Factories;
using ReadyTech.src.Application.Services;

namespace IntegrationTests.Endpoints;

public class CoffeeTests : IClassFixture<CoffeeFactory>
{
    private readonly CoffeeFactory _factory;
    public CoffeeTests(CoffeeFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task BrewCoffee_Returns200_WithMessage()
    {
        var client = _factory.WithFakeService((_, service) =>
        {
            service.Setup(x => x.BrewCoffee())
                   .Returns((200, new { message = "Your piping hot coffee is ready", time = "0000-00-00T00:00:00+00:00" }));
        }).CreateClient();

        var response = await client.GetAsync("/brew-coffee");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("message", body);
        Assert.Contains("time", body);
    }

    [Fact]
    public async Task BrewCoffee_Returns418_OnAprilFools()
    {
        var client = _factory.WithFakeService((_, service) =>
        {
            service.Setup(x => x.BrewCoffee())
                   .Returns((418, null));
        }).CreateClient();

        var response = await client.GetAsync("/brew-coffee");

        Assert.Equal((HttpStatusCode)418, response.StatusCode);
    }

    [Fact]
    public async Task BrewCoffee_Returns503_OnFifthCall()
    {
        var field = typeof(CoffeeService).GetField("CallCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        field?.SetValue(null, 4);

        var client = _factory.WithFakeService((_, service) =>
        {
            service.Setup(x => x.BrewCoffee())
                   .Returns((503, null));
        }).CreateClient();

        var response = await client.GetAsync("/brew-coffee");
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }

    [Fact]
    public async Task BrewCoffee_ThrowsException_ReturnsBadRequest()
    {
        var client = _factory.WithFakeService((_, service) =>
        {
            service.Setup(x => x.BrewCoffee())
                   .Throws(new Exception());
        }).CreateClient();

        var response = await client.GetAsync("/brew-coffee");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
