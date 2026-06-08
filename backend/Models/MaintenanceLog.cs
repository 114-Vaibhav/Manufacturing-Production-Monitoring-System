using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class MaintenanceLog
    {
        [Key]
        public int LogId { get; set; }

        public int MachineId { get; set; }

        public Machine? Machine { get; set; }

        public int EngineerId { get; set; }

        public User? Engineer { get; set; }

        public string IssueDescription { get; set; } = string.Empty;

        public string Resolution { get; set; } = string.Empty;

        public DateTime MaintenanceDate { get; set; }
    }
}