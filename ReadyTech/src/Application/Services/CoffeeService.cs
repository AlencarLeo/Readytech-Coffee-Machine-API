using ReadyTech.src.Application.Interfaces;
namespace ReadyTech.src.Application.Services;

public class CoffeeService : ICoffeeService
{
    private static int CallCount = 0;

    protected virtual bool IsAprilFools()
    {
        var today = DateTime.Now;
        return today.Month == 4 && today.Day == 1;
    }

    public (int StatusCode, object? Response) BrewCoffee()
    {
        CallCount++;

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
            message = "Your piping hot coffee is ready",
            time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz")
        };

        return (200, response);
    }
}