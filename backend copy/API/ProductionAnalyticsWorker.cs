using BusinessLayer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.BackgroundServices
{
    public class ProductionAnalyticsWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProductionAnalyticsWorker> _logger;
        
        // Adjust this interval as needed (e.g., every 5 minutes)
        private readonly TimeSpan _executionInterval = TimeSpan.FromMinutes(5);

        public ProductionAnalyticsWorker(
            IServiceProvider serviceProvider, 
            ILogger<ProductionAnalyticsWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Production Analytics Background Worker is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Background worker triggered: Calculating manufacturing analytics...");

                    // BackgroundServices are singletons, so we must create a scope 
                    // to resolve scoped dependencies like database contexts or service layers.
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var analyticsService = scope.ServiceProvider.GetRequiredService<IProductionAnalyticsServices>();
                        
                        // Execute the internal calculation and update routine
                        await analyticsService.UpdateProductionAnalyticsInternalAsync();
                    }

                    _logger.LogInformation("Manufacturing analytics successfully updated.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while executing the Production Analytics routine.");
                }

                // Wait for the next interval cycle before running again
                await Task.Delay(_executionInterval, stoppingToken);
            }

            _logger.LogInformation("Production Analytics Background Worker is stopping.");
        }
    }
}