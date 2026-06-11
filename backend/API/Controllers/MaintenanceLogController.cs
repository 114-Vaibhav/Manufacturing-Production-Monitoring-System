using backend.Models;
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

        public MaintenanceLogsController(
            IMaintenanceLogServices maintenanceLogServices)
        {
            _maintenanceLogServices = maintenanceLogServices;
        }

        // View all maintenance logs
        [Authorize(Roles =
            "Admin,PlantManager,MaintenanceTechnician," +
            "ProductionManager,QualityInspector,Operator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceLog>>> GetMaintenanceLogs()
        {
            var maintenanceLogs = await _maintenanceLogServices.GetMaintenanceLogs();

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
            MaintenanceLog maintenanceLog)
        {
            var createdMaintenanceLog =
                await _maintenanceLogServices.CreateMaintenanceLog(maintenanceLog);

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
            MaintenanceLog maintenanceLog)
        {
            var updatedMaintenanceLog =
                await _maintenanceLogServices.UpdateMaintenanceLog(id, maintenanceLog);

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

            if (deletedMaintenanceLog == null)
            {
                return NotFound();
            }

            return Ok(deletedMaintenanceLog);
        }
    }
}