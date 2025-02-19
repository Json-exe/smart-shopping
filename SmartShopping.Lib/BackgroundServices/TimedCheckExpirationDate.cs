using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartShopping.Lib.Database;
using SmartShopping.Lib.Services;

namespace SmartShopping.Lib.BackgroundServices;

public sealed class TimedCheckExpirationDate : BackgroundService
{
    private readonly ILogger<TimedCheckExpirationDate> _logger;
    private readonly IDbContextFactory<SmartShoppingDb> _dbContext;
    private readonly NotificationService _notificationService;
    private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromSeconds(5));

    public TimedCheckExpirationDate(ILogger<TimedCheckExpirationDate> logger,
        IDbContextFactory<SmartShoppingDb> dbContext, NotificationService notificationService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _notificationService = notificationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CheckForExpiringFood(stoppingToken);
        while (await _periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            await CheckForExpiringFood(stoppingToken);
        }
    }

    private async Task CheckForExpiringFood(CancellationToken cancellationToken)
    {
        await using var db = await _dbContext.CreateDbContextAsync(cancellationToken);
        var currentDate = DateTime.Now;
        var expiringProducts = await db.Products
            .Where(p => p.NextReminderDate == null || p.NextReminderDate <= currentDate)
            .Where(p => p.ExpirationDate <= currentDate.AddDays(7))
            .Where(p => p.ExpirationDate >= currentDate)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var product in expiringProducts)
        {
            await _notificationService.SendProductExpirationNotification(product);

            var difference = product.ExpirationDate - currentDate;
            product.NextReminderDate = difference.Days switch
            {
                > 3 => product.ExpirationDate.AddDays(-3),
                > 1 => product.ExpirationDate.AddDays(-1),
                > 0 => product.ExpirationDate,
                _ => product.NextReminderDate
            };
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _periodicTimer.Dispose();
    }
}