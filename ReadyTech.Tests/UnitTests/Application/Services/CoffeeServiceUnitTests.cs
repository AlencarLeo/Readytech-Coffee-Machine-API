using ReadyTech.src.Application.Services;

namespace UnitTests.Application.Services;
public class CoffeeServiceUnitTests
{
    [Fact]
    public void BrewCoffee_ShouldReturn200_WhenNotAprilFoolsOrMultipleOf5()
    {
        // Arrange
        var service = new CoffeeService();

        // Act
        var result = service.BrewCoffee();

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Response);
    }

    [Fact]
    public void BrewCoffee_ShouldReturn503_OnEveryFifthCall()
    {
        // Arrange
        var field = typeof(CoffeeService).GetField("CallCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        field?.SetValue(null, 4);

        var service = new CoffeeService();

        // Act
        var result = service.BrewCoffee();

        // Assert
        Assert.Equal(503, result.StatusCode);
    }

    [Fact]
    public void BrewCoffee_ShouldReturn418_OnAprilFools()
    {
        // Arrange
        var service = new TestableCoffeeService(new DateTime(2024, 4, 1));

        // Act
        var result = service.BrewCoffee();

        // Assert
        Assert.Equal(418, result.StatusCode);
    }
}

public class TestableCoffeeService : CoffeeService
{
    private readonly DateTime _dateTime;

    public TestableCoffeeService(DateTime customDate)
    {
        _dateTime = customDate;
    }

    protected override bool IsAprilFools()
    {
        return _dateTime.Month == 4 && _dateTime.Day == 1;
    }
}
