using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class ProductionAnalytics
    {
        [Key]
        public int AnalyticsId { get; set; }

        public int MachineId { get; set; }

        public Machine? Machine { get; set; }

        public double Efficiency { get; set; }

        public double Downtime { get; set; }

        public double DefectRate { get; set; }

        public DateTime CalculatedDate { get; set; }
    }
}