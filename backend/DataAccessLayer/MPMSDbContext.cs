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
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<ProductionAnalytics> ProductionAnalytics { get; set; }
    public DbSet<ProductionOrder> ProductionOrders { get; set; }
    public DbSet<ProductionPlan> ProductionPlans { get; set; }
    public DbSet<ProductionRecord> ProductionRecords { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity< Location>().HasData(
            new Location
            {
                LocationId = 1,
                LocationName = "Assembly Line",
                FloorNo = "1",
                TerminalNo = "A1",
                Status = LocationStatus.Active
            },
            new Location
            {
                LocationId = 2,
                LocationName = "Packaging Line",
                FloorNo = "1",
                TerminalNo = "A2",
                Status = LocationStatus.Active
            },
            new Location
            {
                LocationId = 3,
                LocationName = "Quality Control",
                FloorNo = "1",
                TerminalNo = "A3",
                Status = LocationStatus.Active
            }
        );
       
        modelBuilder.Entity<DowntimeReason>().HasData(
            new DowntimeReason { Id = 1, Name = "Machine Breakdown" },
            new DowntimeReason { Id = 2, Name = "Power Failure" },
            new DowntimeReason { Id = 3, Name = "Material Shortage" },
            new DowntimeReason { Id = 4, Name = "Tool Change" },
            new DowntimeReason { Id = 5, Name = "Planned Maintenance" },
            new DowntimeReason { Id = 6, Name = "Unplanned Maintenance" },
            new DowntimeReason { Id = 7, Name = "Operator Absence" },
            new DowntimeReason { Id = 8, Name = "Quality Issue" },
            new DowntimeReason { Id = 9, Name = "Setup Changeover" },
            new DowntimeReason { Id = 10, Name = "Network Failure" }
        );

        modelBuilder.Entity<Machine>().HasData(
            new Machine
            {
                MachineId = 1,
                MachineCode = "ASM-001",
                MachineName = "Conveyor Belt A",
                LocationId = 1
            },
            new Machine
            {
                MachineId = 2,
                MachineCode = "ASM-002",
                MachineName = "Robotic Arm A",
                LocationId = 1
            },
            new Machine
            {
                MachineId = 3,
                MachineCode = "PKG-001",
                MachineName = "Packaging Machine A",
                LocationId = 2
            },
            new Machine
            {
                MachineId = 4,
                MachineCode = "PKG-002",
                MachineName = "Labeling Machine A",
                LocationId = 2
            },
            new Machine
            {
                MachineId = 5,
                MachineCode = "QC-001",
                MachineName = "Vision Inspection System",
                LocationId = 3
            }
        );
    }

}
}
