namespace ReadyTech.src.Application.Interfaces;
public interface ICoffeeService
{
    (int StatusCode, object? Response) BrewCoffee();
}