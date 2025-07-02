using System.Net;

using IntegrationTests.Factories;

using Moq;

using ReadyTech.src.Application.Interfaces;
using ReadyTech.src.Application.Services;

namespace IntegrationTests.Endpoints;

public class CoffeeTests : IClassFixture<WithMockService>
{
    private readonly WithMockService _factory;
    public CoffeeTests(WithMockService factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData(40, "iced coffee")]
    [InlineData(20, "piping hot")]
    public async Task BrewCoffee_ReturnsExpectedMessage_BasedOnTemperature(double temp, string expectedMessagePart)
    {
        var client = _factory.WithMock<IOpenWeatherMapService>((_, mock) =>
        {
            mock.Setup(x => x.GetCurrentTemperatureAsync("Melbourne"))
                .ReturnsAsync(temp);
        }).CreateClient();

        var response = await client.GetAsync("/brew-coffee");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(expectedMessagePart, content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task BrewCoffee_Returns200_WithMessage()
    {
        var client = _factory.WithMock<ICoffeeService>((_, service) =>
        {
            service.Setup(x => x.BrewCoffee())
                   .ReturnsAsync((200, new { message = "Your piping hot coffee is ready", time = "0000-00-00T00:00:00+00:00" }));
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
        var client = _factory.WithMock<ICoffeeService>((_, service) =>
        {
            service.Setup(x => x.BrewCoffee())
                   .ReturnsAsync((418, null));
        }).CreateClient();

        var response = await client.GetAsync("/brew-coffee");

        Assert.Equal((HttpStatusCode)418, response.StatusCode);
    }

    [Fact]
    public async Task BrewCoffee_Returns503_OnFifthCall()
    {
        var field = typeof(CoffeeService).GetField("CallCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        field?.SetValue(null, 4);

        var client = _factory.WithMock<ICoffeeService>((_, service) =>
        {
            service.Setup(x => x.BrewCoffee())
                   .ReturnsAsync((503, null));
        }).CreateClient();

        var response = await client.GetAsync("/brew-coffee");
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }

    [Fact]
    public async Task BrewCoffee_ThrowsException_ReturnsBadRequest()
    {
        var client = _factory.WithMock<ICoffeeService>((_, service) =>
        {
            service.Setup(x => x.BrewCoffee())
                   .Throws(new Exception());
        }).CreateClient();

        var response = await client.GetAsync("/brew-coffee");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

}
