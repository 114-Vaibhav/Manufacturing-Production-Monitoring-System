namespace API.Services
{
    public interface IAuditLogService
    {
        void Add(string userName, string action, string entityName, int? entityId);
        IReadOnlyList<AuditLogEntry> GetRecent();
    }
}
