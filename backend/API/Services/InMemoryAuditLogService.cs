namespace API.Services
{
    public class InMemoryAuditLogService : IAuditLogService
    {
        private readonly List<AuditLogEntry> _entries = new();
        private readonly object _lock = new();

        public void Add(string userName, string action, string entityName, int? entityId)
        {
            lock (_lock)
            {
                _entries.Add(new AuditLogEntry
                {
                    CreatedAt = DateTime.UtcNow,
                    UserName = string.IsNullOrWhiteSpace(userName) ? "Unknown" : userName,
                    Action = action,
                    EntityName = entityName,
                    EntityId = entityId
                });
            }
        }

        public IReadOnlyList<AuditLogEntry> GetRecent()
        {
            lock (_lock)
            {
                return _entries
                    .OrderByDescending(entry => entry.CreatedAt)
                    .Take(100)
                    .ToList();
            }
        }
    }
}
