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
}