using ReadyTech.src.Application.Interfaces;
using System.Text.Json;

namespace ReadyTech.src.Application.Services;

public class OpenWeatherMapService : IOpenWeatherMapService
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;
    private readonly string _units = "metric";

    public OpenWeatherMapService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["WeatherApi:ApiKey"];
    }

    public async Task<double?> GetCurrentTemperatureAsync(string city)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city},AU&appid={_apiKey}&units={_units}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(content);

            var temp = json.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
            return temp;
        }
        catch
        {
            return null;
        }
    }
}
