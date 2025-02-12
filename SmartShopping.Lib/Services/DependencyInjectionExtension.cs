using Microsoft.Extensions.DependencyInjection;

namespace SmartShopping.Lib.Services;

internal static class DependencyInjectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddSingleton<NotificationService>();
    }
}