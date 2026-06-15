using API.Services;
using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceLogsController : ControllerBase
    {
        private readonly IMaintenanceLogServices _maintenanceLogServices;
        private readonly IAuditLogService _auditLogService;

        public MaintenanceLogsController(
            IMaintenanceLogServices maintenanceLogServices,
            IAuditLogService auditLogService)
        {
            _maintenanceLogServices = maintenanceLogServices;
            _auditLogService = auditLogService;
        }

        // View all maintenance logs
        [Authorize(Roles =
            "Admin,PlantManager,MaintenanceTechnician," +
            "ProductionManager,QualityInspector,Operator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceLog>>> GetMaintenanceLogs(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var maintenanceLogs = await _maintenanceLogServices.GetMaintenanceLogs(pageNumber, pageSize);

            return Ok(maintenanceLogs);
        }

        // View a specific maintenance log
        [Authorize(Roles =
            "Admin,PlantManager,MaintenanceTechnician," +
            "ProductionManager,QualityInspector,Operator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceLog>> GetMaintenanceLog(int id)
        {
            var maintenanceLog = await _maintenanceLogServices.GetMaintenanceLog(id);

            if (maintenanceLog == null)
            {
                return NotFound();
            }

            return Ok(maintenanceLog);
        }

        // Create a maintenance log
        [Authorize(Roles =
            "Admin,PlantManager,MaintenanceTechnician")]
        [HttpPost]
        public async Task<ActionResult<MaintenanceLog>> PostMaintenanceLog(
            MaintenanceLogRequest maintenanceLog)
        {
            var createdMaintenanceLog =
                await _maintenanceLogServices.CreateMaintenanceLog(maintenanceLog, this.GetCurrentUserId());
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Create", nameof(MaintenanceLog), createdMaintenanceLog.LogId);

            return CreatedAtAction(
                nameof(GetMaintenanceLog),
                new { id = createdMaintenanceLog.LogId },
                createdMaintenanceLog);
        }

        // Update a maintenance log
        [Authorize(Roles =
            "Admin,PlantManager,MaintenanceTechnician")]
        [HttpPut("{id}")]
        public async Task<ActionResult<MaintenanceLog>> PutMaintenanceLog(
            int id,
            MaintenanceLogRequest maintenanceLog)
        {
            var updatedMaintenanceLog =
                await _maintenanceLogServices.UpdateMaintenanceLog(id, maintenanceLog, this.GetCurrentUserId());
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Update", nameof(MaintenanceLog), id);

            if (updatedMaintenanceLog == null)
            {
                return NotFound();
            }

            return Ok(updatedMaintenanceLog);
        }

        // Delete a maintenance log
        [Authorize(Roles = "Admin,PlantManager")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MaintenanceLog>> DeleteMaintenanceLog(int id)
        {
            var deletedMaintenanceLog =
                await _maintenanceLogServices.DeleteMaintenanceLog(id);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Delete", nameof(MaintenanceLog), id);

            if (deletedMaintenanceLog == null)
            {
                return NotFound();
            }

            return Ok(deletedMaintenanceLog);
        }
    }
}
