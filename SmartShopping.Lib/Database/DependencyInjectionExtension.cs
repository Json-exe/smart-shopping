using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SmartShopping.Lib.Database;

internal static class DependencyInjectionExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        return services.AddDbContextFactory<SmartShoppingDb>(c =>
        {
            c.UseSqlite("smart-shopping.db");
        });
    }
}