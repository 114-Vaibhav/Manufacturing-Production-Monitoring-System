using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public enum ProductionPlanStatus
    {
        Planned,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }
    public class ProductionPlan
    {
        [Key]
        public int PlanId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int TargetQuantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ProductionPlanStatus Status { get; set; }

        public int CreatedBy { get; set; }

        public User? Creator { get; set; }

        public ICollection<ProductionOrder>? ProductionOrders { get; set; }
    }
}