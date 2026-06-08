using System;
using System.ComponentModel.DataAnnotations;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.DataAccessLayer{
    
public class MPMSDbContext : DbContext
{
    public MPMSDbContext(
        DbContextOptions<MPMSDbContext> options)
        : base(options)
    {
    }
    public DbSet<Defect> Defects { get; set; }
    public DbSet<Machine> Machines { get; set; }
    public DbSet<MachineReading> MachineReadings { get; set; }
    public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
    public DbSet<ProductionAnalytics> ProductionAnalyticss { get; set; }
    public DbSet<ProductionOrder> ProductionOrders { get; set; }
    public DbSet<ProductionPlan> ProductionPlans { get; set; }
    public DbSet<ProductionRecord> ProductionRecords { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<User> Users { get; set; }


}
}
