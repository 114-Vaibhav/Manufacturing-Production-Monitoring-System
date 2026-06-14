using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/admin/logs")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AdminLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<AuditLogEntry>> GetLogs()
        {
            return Ok(_auditLogService.GetRecent());
        }
    }
}
