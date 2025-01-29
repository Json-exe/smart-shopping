using Microsoft.Extensions.DependencyInjection;

namespace SmartShopping.Lib.ViewModel;

internal static class DependencyInjectionExtension
{
    public static IServiceCollection AddViewModel(this IServiceCollection services)
    {
        return services.AddTransient<ProductViewViewModel>();
    }
}