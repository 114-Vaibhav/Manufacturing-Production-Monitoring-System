using IoTSensors;
using backend.DataAccessLayer;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Services
{
public class IoTSensorService : IIoTSensorService
{
    private readonly MPMSDbContext _context;

    private readonly TemperatureSensor _temperatureSensor = new();
    private readonly VibrationSensor _vibrationSensor = new();
    private readonly PowerSensor _powerSensor = new();

    public IoTSensorService(MPMSDbContext context)
    {
        _context = context;
    }

    public async Task GenerateMachineReadingsAsync()
    {
        var machines = await _context.Machines.ToListAsync();

        foreach (var machine in machines)
        {
            var reading = new MachineReading
            {
                MachineId = machine.MachineId,
                Temperature = _temperatureSensor.Read(),
                Vibration = _vibrationSensor.Read(),
                PowerConsumption = _powerSensor.Read(),
                Timestamp = DateTime.UtcNow
            };

            _context.MachineReadings.Add(reading);
        }

        await _context.SaveChangesAsync();
    }
}
}