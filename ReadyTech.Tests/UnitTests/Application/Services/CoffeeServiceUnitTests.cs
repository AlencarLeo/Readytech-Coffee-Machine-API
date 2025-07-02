using System.Reflection;

using Moq;

using ReadyTech.src.Application.Interfaces;
using ReadyTech.src.Application.Services;

namespace UnitTests.Application.Services;

public class CoffeeServiceUnitTests
{
    private void ResetCallCount()
    {
        var field = typeof(CoffeeService).GetField("CallCount", BindingFlags.NonPublic | BindingFlags.Static);
        field?.SetValue(null, 0);
    }

    private CoffeeService CreateService(int? temperature, bool isAprilFools = false)
    {
        var weatherMock = new Mock<IOpenWeatherMapService>();
        weatherMock.Setup(x => x.GetCurrentTemperatureAsync("Melbourne"))
                    .ReturnsAsync(temperature);

        var service = new TestableCoffeeService(weatherMock.Object, isAprilFools);
        return service;
    }

    private class TestableCoffeeService : CoffeeService
    {
        private readonly bool _isAprilFoolsOverride;

        public TestableCoffeeService(IOpenWeatherMapService weatherService, bool isAprilFools)
            : base(weatherService)
        {
            _isAprilFoolsOverride = isAprilFools;
        }

        protected override bool IsAprilFools() => _isAprilFoolsOverride;
    }

    [Theory]
    [InlineData(20, "piping hot")]
    [InlineData(40, "refreshing iced")]
    public async Task BrewCoffee_Returns200_WithCorrectMessage_BasedOnTemperature(int temperature, string expectedMessage)
    {
        // Arrange
        ResetCallCount();
        var service = CreateService(temperature);

        // Act
        var result = await service.BrewCoffee();

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Response);
        Assert.Contains(expectedMessage, result.Response!.ToString());
    }

    [Theory]
    [InlineData(20)]
    [InlineData(40)]
    public async Task BrewCoffee_ShouldReturn503_OnEveryFifthCall(int temperature)
    {
        // Arrange
        var field = typeof(CoffeeService).GetField("CallCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        field?.SetValue(null, 4);

        var service = CreateService(temperature);

        // Act
        var result = await service.BrewCoffee();

        // Assert
        Assert.Equal(503, result.StatusCode);
        Assert.Null(result.Response);
    }

    [Theory]
    [InlineData(20)]
    [InlineData(40)]
    public async Task BrewCoffee_ShouldReturn418_OnAprilFools(int temperature)
    {
        // Arrange
        ResetCallCount();
        var service = CreateService(temperature, isAprilFools: true);

        // Act
        var result = await service.BrewCoffee();

        // Assert
        Assert.Equal(418, result.StatusCode);
        Assert.Null(result.Response);
    }

    [Fact]
    public async Task BrewCoffee_ShouldReturn500_WhenWeatherServiceReturnsNull()
    {
        // Arrange
        ResetCallCount();
        var service = CreateService(null, isAprilFools: false);

        // Act
        var result = await service.BrewCoffee();

        // Assert
        Assert.Equal(500, result.StatusCode);
        Assert.NotNull(result.Response);
        Assert.Contains("Weather service is unavailable or invalid API key.", result.Response!.ToString());
    }

}
