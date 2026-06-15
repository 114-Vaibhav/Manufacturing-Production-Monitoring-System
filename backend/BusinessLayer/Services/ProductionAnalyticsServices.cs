using backend.Models;
using BusinessLayer.Interfaces;
using backend.DataAccessLayer; // 1. Import your Data Access Layer namespace
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessLayer.Services
{
    public class ProductionAnalyticsServices : IProductionAnalyticsServices
    {
        // 2. Declare a private readonly field for your DB context
        private readonly MPMSDbContext _context; 

        // 3. Inject it through the constructor
        public ProductionAnalyticsServices(MPMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductionAnalytics>> GetProductionAnalytics(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            return await _context.ProductionAnalytics
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateProductionAnalyticsInternalAsync()
        {
            // Fetch your active machines
            var activeMachines = await _context.Machines.ToListAsync();

            foreach (var machine in activeMachines)
            {
                var thresholdDate = DateTime.UtcNow.AddDays(-1);

                var readings = await _context.MachineReadings
                    .Where(r => r.MachineId == machine.MachineId && r.Timestamp >= thresholdDate)
                    .ToListAsync();

                var defects = await _context.Defects
                    .Where(d => d.MachineId == machine.MachineId && d.CreatedAt >= thresholdDate)
                    .ToListAsync();

                // Optimized relationship mapping based on your models
                var records = await _context.ProductionRecords
                    .Where(pr => pr.ProductionPlanId == machine.MachineId) // Adjust based on how you trace records to a machine
                    .ToListAsync();

                double calculatedEfficiency = CalculateMachineEfficiency(readings, records);
                double calculatedDowntime = CalculateMachineDowntime(readings);
                double calculatedDefectRate = CalculateMachineDefectRate(defects, records);

                var today = DateTime.UtcNow.Date;
                var existingAnalytics = await _context.ProductionAnalytics
                    .FirstOrDefaultAsync(a => a.MachineId == machine.MachineId && a.CalculatedDate.Date == today);

                if (existingAnalytics != null)
                {
                    existingAnalytics.Efficiency = calculatedEfficiency;
                    existingAnalytics.Downtime = calculatedDowntime;
                    existingAnalytics.DefectRate = calculatedDefectRate;
                    existingAnalytics.CalculatedDate = DateTime.UtcNow;
                }
                else
                {
                    var newAnalytics = new ProductionAnalytics
                    {
                        MachineId = machine.MachineId,
                        Efficiency = calculatedEfficiency,
                        Downtime = calculatedDowntime,
                        DefectRate = calculatedDefectRate,
                        CalculatedDate = DateTime.UtcNow
                    };
                    await _context.ProductionAnalytics.AddAsync(newAnalytics);
                }
            }

            await _context.SaveChangesAsync();
        }

        private double CalculateMachineEfficiency(List<MachineReading> readings, List<ProductionRecord> records)
        {
            return readings.Any() ? 85.5 : 0.0; 
        }

        private double CalculateMachineDowntime(List<MachineReading> readings)
        {
            return readings.Count(r => r.PowerConsumption < 5) * 5; 
        }

        private double CalculateMachineDefectRate(List<Defect> defects, List<ProductionRecord> records)
        {
            return 1.2; 
        }
    }
}