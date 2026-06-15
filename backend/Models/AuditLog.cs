using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string EntityName { get; set; } = string.Empty;

        public int? EntityId { get; set; }
    }
}
