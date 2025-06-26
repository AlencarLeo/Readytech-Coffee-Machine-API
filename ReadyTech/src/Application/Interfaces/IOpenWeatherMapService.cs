namespace ReadyTech.src.Application.Interfaces;
public interface IOpenWeatherMapService
{
    Task<double?> GetCurrentTemperatureAsync(string city);
}
