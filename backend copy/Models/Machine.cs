using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
  
    public class Machine
    {
        [Key]
        public int MachineId { get; set; }

        public string MachineName { get; set; } = string.Empty;

        public string MachineCode { get; set; } = string.Empty;

        public int LocationId { get; set; }

        public MachineStatus Status { get; set; }

        public DateTime? LastMaintenanceDate { get; set; }


        public ICollection<MachineReading>? MachineReadings { get; set; }

        public ICollection<ProductionOrder>? ProductionOrders { get; set; }

        public ICollection<Defect>? Defects { get; set; }

        public ICollection<MaintenanceLog>? MaintenanceLogs { get; set; }

        public ICollection<ProductionAnalytics>? Analytics { get; set; }

        public Location? Location { get; set; }
    }
}