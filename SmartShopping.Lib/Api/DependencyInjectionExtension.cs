using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace SmartShopping.Lib.Api;

internal static class DependencyInjectionExtension
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        return services.AddTransient<OpenFoodFactsApiClient>()
            .AddHttpClient<OpenFoodFactsApiClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new UriBuilder
                {
                    Host = "world.openfoodfacts.net",
                    Scheme = Uri.UriSchemeHttps,
                    Path = "api/v2"
                }.Uri;
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("SmartShopping", "0.0.1"));
            })
            .Services;
    }
}