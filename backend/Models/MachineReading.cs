using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class MachineReading
    {
        [Key]
        public int ReadingId { get; set; }

        public int MachineId { get; set; }

        public Machine? Machine { get; set; }

        public double Temperature { get; set; }

        public double Vibration { get; set; }

        public double PowerConsumption { get; set; }

        public DateTime Timestamp { get; set; }
    }
}