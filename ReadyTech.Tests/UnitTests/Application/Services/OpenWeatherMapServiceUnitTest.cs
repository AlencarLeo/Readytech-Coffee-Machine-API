using Microsoft.Extensions.Configuration;

using ReadyTech.src.Application.Services;

using UnitTestsmock;

namespace UnitTests.Application.Services;

public class OpenWeatherMapServiceUnitTest
{
    [Fact]
    public async Task GetCurrentTemperatureAsync_ReturnsTemperature_WhenResponseIsSuccessful()
    {
        // Arrange
        var city = "Melbourne";
        var expectedTemp = 25.5;
        var json = $@"{{ ""main"": {{ ""temp"": {expectedTemp} }} }}";

        var settings = new Dictionary<string, string?> {
            {"WeatherApi:ApiKey", "fake-api-key"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        var httpClient = HttpClientFactory.CreateWithResponse(json);

        var service = new OpenWeatherMapService(httpClient, configuration);

        // Act
        var result = await service.GetCurrentTemperatureAsync(city);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTemp, result);
    }
}
