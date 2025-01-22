using Microsoft.Extensions.DependencyInjection;
using SmartShopping.Lib.Api;
using SmartShopping.Lib.Database;

namespace SmartShopping.Lib;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddLib(this IServiceCollection services)
    {
        return services.AddApi()
            .AddDatabase();
    }
}