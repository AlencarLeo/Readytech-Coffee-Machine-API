using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using ReadyTech.src.Application.Interfaces;

namespace IntegrationTests.Factories;
public class CoffeeFactory : WebApplicationFactory<Program>
{
    public WebApplicationFactory<Program> WithFakeService(Action<IServiceCollection, Mock<ICoffeeService>> config)
    {
        var mockService = new Mock<ICoffeeService>();

        return this.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(ICoffeeService));
                config(services, mockService);
                services.AddSingleton(mockService.Object);
            });
        });
    }
}
