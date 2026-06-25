using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public enum MachineStatus
    {
        Running,
        Idle,
        Maintenance,
        OutOfService
    }
    public enum ProductionOrderStatus
    {
        Planned,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }

    public enum ShiftType
    {
        Morning,
        Evening,
        Night
    }
        public enum DefectSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
    public enum DefectType
    {
        Mechanical,
        Electrical,
        Software,
        Quality
    }
    public enum LocationStatus
    {
        Active,
        Inactive,
        UnderMaintenance
    }
    public enum ProductStatus
    {
        Active,
        Discontinued,
        OutOfStock
    }
    public enum ProductionPlanStatus
    {
        Planned,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }
    public enum UserRole
    {
        Admin,
        Operator,
        ProductionManager,
        QualityInspector,
        MaintenanceTechnician,
        ProductionPlanner,
        PlantManager

    }
    public enum UserStatus
    {
        Active,
        Inactive,
        Suspended
    }
}