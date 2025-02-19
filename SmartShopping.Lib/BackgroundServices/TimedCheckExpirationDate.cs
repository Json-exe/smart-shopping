using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartShopping.Lib.Database;

namespace SmartShopping.Lib.BackgroundServices;

public class TimedCheckExpirationDate : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILogger<TimedCheckExpirationDate> _logger;
    private readonly IDbContextFactory<SmartShoppingDb> _dbContext;
    
    public TimedCheckExpirationDate(ILogger<TimedCheckExpirationDate> logger, IDbContextFactory<SmartShoppingDb> dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Background Service is starting.");
        
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        
        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        _logger.LogInformation("Timed Background Service is working.");
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Background Service is stopping.");
        
        _timer?.Change(Timeout.Infinite, 0);
        
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        _timer?.Dispose();
    }
}