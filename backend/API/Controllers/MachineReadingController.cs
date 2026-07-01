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
    public class MachineReadingsController : ControllerBase
    {
        private readonly IMachineReadingServices _machineReadingServices;
        private readonly IAuditLogService _auditLogService;

        public MachineReadingsController(
            IMachineReadingServices machineReadingServices,
            IAuditLogService auditLogService)
        {
            _machineReadingServices = machineReadingServices;
            _auditLogService = auditLogService;
        }

        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager," +
            "ProductionPlanner,QualityInspector," +
            "MaintenanceTechnician,Operator")]
            [HttpGet] // <-- Removed the "{pageNumber}/{pageSize}" template
        public async Task<ActionResult<IEnumerable<MachineReading>>> GetMachineReadings(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var machineReadings = await _machineReadingServices.GetMachineReadings(pageNumber, pageSize);

            return Ok(machineReadings);
        }
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<MachineReading>>> GetMachineReadings(
        //     [FromQuery] int pageNumber = 1,
        //     [FromQuery] int pageSize = 10)
        // {
        //     var machineReadings = await _machineReadingServices.GetMachineReadings(pageNumber, pageSize);

        //     return Ok(machineReadings);
        // }

        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager," +
            "ProductionPlanner,QualityInspector," +
            "MaintenanceTechnician,Operator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MachineReading>> GetMachineReading(int id)
        {
            var machineReading = await _machineReadingServices.GetMachineReading(id);
            Console.WriteLine("Sending machine readings");
            if (machineReading == null)
            {
                return NotFound();
            }

            return Ok(machineReading);
        }

        // [Authorize(Roles =
        //     "Admin,PlantManager,ProductionManager," +
        //     "MaintenanceTechnician,Operator")]
        // [HttpPost]
        // public async Task<ActionResult<MachineReading>> PostMachineReading(
        //     MachineReadingRequest machineReading)
        // {
        //     var createdMachineReading =
        //         await _machineReadingServices.CreateMachineReading(machineReading);
        //     _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Create", nameof(MachineReading), createdMachineReading.ReadingId);

        //     return CreatedAtAction(
        //         nameof(GetMachineReading),
        //         new { id = createdMachineReading.ReadingId },
        //         createdMachineReading);
        // }

        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager," +
            "MaintenanceTechnician")]
        [HttpPut("{id}")]
        public async Task<ActionResult<MachineReading>> PutMachineReading(int id, MachineReadingRequest machineReading)
        {
            try
            {
                Console.WriteLine($"[DEBUG] Starting update for ID: {id}");
                
                var updatedMachineReading = await _machineReadingServices.UpdateMachineReading(id, machineReading);
                Console.WriteLine("[DEBUG] Update finished.");

                _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Update", nameof(MachineReading), id);
                Console.WriteLine("[DEBUG] Audit log added.");

                if (updatedMachineReading == null) return NotFound();

                return Ok(updatedMachineReading);
            }
            catch (Exception ex)
            {
                // This forces the exact error to print to your terminal
                Console.WriteLine($"[CRITICAL ERROR]: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw; 
            }
        }
        // [HttpPut("{id}")]
        // public async Task<ActionResult<MachineReading>> PutMachineReading(
        //     int id,
        //     MachineReadingRequest machineReading)
        // {
        //     var updatedMachineReading =
        //         await _machineReadingServices.UpdateMachineReading(id, machineReading);
        //     _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Update", nameof(MachineReading), id);

        //     Console.WriteLine("Updating machine readings");
        //     if (updatedMachineReading == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(updatedMachineReading);
        // }

        [Authorize(Roles = "Admin,PlantManager")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MachineReading>> DeleteMachineReading(int id)
        {
            var deletedMachineReading =
                await _machineReadingServices.DeleteMachineReading(id);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Delete", nameof(MachineReading), id);

            if (deletedMachineReading == null)
            {
                return NotFound();
            }

            return Ok(deletedMachineReading);
        }
    }
}
