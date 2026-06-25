namespace backend.Models.DTOs
{
    public class MachineRequest
    {
        public string MachineName { get; set; } = string.Empty;
        public string MachineCode { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public MachineStatus Status { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
    }

    public class MachineReadingRequest
    {
        public int MachineId { get; set; }
        public double Temperature { get; set; }
        public double Vibration { get; set; }
        public double PowerConsumption { get; set; }
    }

    public class MaintenanceLogRequest
    {
        public int MachineId { get; set; }
        public string IssueDescription { get; set; } = string.Empty;
        public string Resolution { get; set; } = string.Empty;
    }

    public class DefectRequest
    {
        public int OrderId { get; set; }
        public int MachineId { get; set; }
        public DefectType Type { get; set; }
        public DefectSeverity Severity { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class ProductRequest
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ProductionOrderRequest
    {
        public int PlanId { get; set; }
        public int MachineId { get; set; }
        public int Quantity { get; set; }
        public int ProducedQuantity { get; set; }
        public ProductionOrderStatus Status { get; set; }
    }

    public class ProductionPlanRequest
    {
        public string ProductName { get; set; } = string.Empty;
        public int TargetQuantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProductionPlanStatus Status { get; set; }
    }

    public class ProductionRecordRequest
    {
        public int ProductionPlanId { get; set; }
        public int ProducedQuantity { get; set; }
    }

    public class UpdatePasswordRequest
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

}
