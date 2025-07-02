using ReadyTech.src.Application.Interfaces;
namespace ReadyTech.src.Application.Services;

public class CoffeeService : ICoffeeService
{
    private static int CallCount = 0;
    private readonly IOpenWeatherMapService _openWeatherMapService;
    public CoffeeService(IOpenWeatherMapService openWeatherMapService)
    {
        _openWeatherMapService = openWeatherMapService;
    }

    protected virtual bool IsAprilFools()
    {
        var today = DateTime.Now;
        return today.Month == 4 && today.Day == 1;
    }

    public async Task<(int StatusCode, object? Response)> BrewCoffee()
    {
        CallCount++;

        var result = await _openWeatherMapService.GetCurrentTemperatureAsync("Melbourne");

        if (result == null)
        {
            return (500, new { error = "Weather service is unavailable or invalid API key." });
        }

        if (CallCount % 5 == 0)
        {
            return (503, null);
        }

        if (IsAprilFools())
        {
            return (418, null);
        }

        var response = new
        {
            message = result > 30 ? "Your refreshing iced coffee is ready" : "Your piping hot coffee is ready",
            time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz")
        };

        return (200, response);
    }
}
