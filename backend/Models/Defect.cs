using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{

    public class Defect
    {
        [Key]
        public int DefectId { get; set; }

        public int OrderId { get; set; }

        public ProductionOrder? ProductionOrder { get; set; }

        public int MachineId { get; set; }

        public Machine? Machine { get; set; }

        public DefectType Type { get; set; }

        public DefectSeverity Severity { get; set; }

        public string Description { get; set; } = string.Empty;

        public int ReportedBy { get; set; }

        public User? Reporter { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}