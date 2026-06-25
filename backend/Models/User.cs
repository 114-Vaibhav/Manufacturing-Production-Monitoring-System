using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{

    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] HashKey { get; set; } = Array.Empty<byte>();
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserStatus Status { get; set; }

        public ICollection<ProductionPlan>? ProductionPlans { get; set; }

        public ICollection<Defect>? ReportedDefects { get; set; }
    }
}