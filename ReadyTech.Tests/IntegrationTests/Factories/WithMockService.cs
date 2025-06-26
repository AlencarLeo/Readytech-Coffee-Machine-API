using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace IntegrationTests.Factories;
public class WithMockService : WebApplicationFactory<Program>
{
    public WebApplicationFactory<Program> WithMock<T>(Action<IServiceCollection, Mock<T>> config) where T : class
    {
        var mock = new Mock<T>();

        return this.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(T));
                config(services, mock);
                services.AddSingleton(mock.Object);
            });
        });
    }
}