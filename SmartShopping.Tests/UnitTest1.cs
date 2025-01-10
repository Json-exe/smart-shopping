using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SmartShopping.Lib;
using SmartShopping.Lib.Api;

namespace SmartShopping.Tests;

public class UnitTest1
{
    private readonly ServiceProvider _services = new ServiceCollection()
        .AddLib()
        .BuildServiceProvider();
    
    [Fact]
    public async Task TestOpenFoodFactsApiAndJsonDeserialization()
    {
        var client = _services.GetRequiredService<OpenFoodFactsApiClient>();
        var response = await client.GetProductInformation("3017620422003");
        response.Code.Should().Be("3017620422003");
        response.Product.ProductName.Should().Contain("Nutella");
        response.Product.Ingredients.Should().NotBeEmpty();
        response.Status.Should().Be(1);
    }
}