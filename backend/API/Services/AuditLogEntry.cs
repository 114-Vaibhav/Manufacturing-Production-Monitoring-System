namespace API.Services
{
    public class AuditLogEntry
    {
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public int? EntityId { get; set; }
    }
}
