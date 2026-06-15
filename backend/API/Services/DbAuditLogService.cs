using backend.DataAccessLayer;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class DbAuditLogService : IAuditLogService
    {
        private readonly MPMSDbContext _context;

        public DbAuditLogService(MPMSDbContext context)
        {
            _context = context;
        }

        public void Add(string userName, string action, string entityName, int? entityId)
        {
            var audit = new AuditLog
            {
                CreatedAt = DateTime.UtcNow,
                UserName = string.IsNullOrWhiteSpace(userName) ? "Unknown" : userName,
                Action = action,
                EntityName = entityName,
                EntityId = entityId
            };

            _context.AuditLogs.Add(audit);
            _context.SaveChanges();
        }

        public IReadOnlyList<AuditLogEntry> GetRecent()
        {
            return _context.AuditLogs
                .OrderByDescending(entry => entry.CreatedAt)
                .Take(100)
                .Select(entry => new AuditLogEntry
                {
                    CreatedAt = entry.CreatedAt,
                    UserName = entry.UserName,
                    Action = entry.Action,
                    EntityName = entry.EntityName,
                    EntityId = entry.EntityId
                })
                .ToList();
        }
    }
}
