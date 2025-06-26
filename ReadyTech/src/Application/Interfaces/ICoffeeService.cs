namespace ReadyTech.src.Application.Interfaces;
public interface ICoffeeService
{
    Task<(int StatusCode, object? Response)> BrewCoffee();
}