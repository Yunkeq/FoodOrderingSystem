using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FoodOrderingSystem.IntegrationTests;

public class BaseIntegrationTest : IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;
    public BaseIntegrationTest(IntegrationTestFactory factory)
    {
        _factory = factory;
        ServiceScope = _factory.Services.CreateScope();
        HttpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            HandleCookies = true,
            BaseAddress = new Uri("https://localhost"), // for cookie setting
        });
    }

    protected HttpClient HttpClient { get; private set; }
    protected IServiceScope ServiceScope { get; private set; }
}
