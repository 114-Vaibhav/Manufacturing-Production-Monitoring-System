using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public enum ProductionOrderStatus
    {
        Planned,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }
    public class ProductionOrder
    {
        [Key]
        public int OrderId { get; set; }

        public int PlanId { get; set; }

        public ProductionPlan? ProductionPlan { get; set; }

        public int MachineId { get; set; }

        public Machine? Machine { get; set; }

        public int Quantity { get; set; }

        public int ProducedQuantity { get; set; }

        public ProductionOrderStatus Status { get; set; }

        public ICollection<Defect>? Defects { get; set; }
    }
}