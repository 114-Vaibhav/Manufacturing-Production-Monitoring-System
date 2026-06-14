using BusinessLayer.Interfaces;

namespace API.BackgroundServices
{
    public class MachineSensorSimulatorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MachineSensorSimulatorService> _logger;

        public MachineSensorSimulatorService(
            IServiceScopeFactory scopeFactory,
            ILogger<MachineSensorSimulatorService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var sensorService =
                    scope.ServiceProvider.GetRequiredService<IIoTSensorService>();

                await sensorService.GenerateMachineReadingsAsync();

                _logger.LogInformation(
                    "Sensor readings generated at {time}",
                    DateTime.Now);

                await Task.Delay(
                    TimeSpan.FromSeconds(30),
                    stoppingToken);
            }
        }
    }
}