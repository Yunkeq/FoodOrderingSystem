using FoodOrderingSystem.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoodOrderingSystem.Infrastructure.BackgroundServices;

public sealed class ExpiredRefreshTokensCleanupBackgroundService : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(6);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ExpiredRefreshTokensCleanupBackgroundService> _logger;

    public ExpiredRefreshTokensCleanupBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<ExpiredRefreshTokensCleanupBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogWarning("{Service} started.", nameof(ExpiredRefreshTokensCleanupBackgroundService));
            try
            {
                await RemoveRefreshTokens(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // normal shutdown
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to cleanup expired refresh tokens.");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }

    private async Task RemoveRefreshTokens(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var now = DateTime.UtcNow;

        var deleted = await db.RefreshTokens
            .Where(rt => rt.ExpirationDate < now)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleted > 0)
        {
            _logger.LogWarning("Deleted {Count} expired refresh tokens.", deleted);
        }
    }
}