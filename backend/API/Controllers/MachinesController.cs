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
    public class MachinesController : ControllerBase
    {
        private IMachineServices _machineServices;
        private readonly IAuditLogService _auditLogService;

        public MachinesController(
            IMachineServices machineServices,
            IAuditLogService auditLogService)
        {
            _machineServices = machineServices;
            _auditLogService = auditLogService;
        }

        [Authorize(Roles =
        "Admin,PlantManager,ProductionManager," +
        "ProductionPlanner,QualityInspector," +
        "MaintenanceTechnician,Operator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var machines = await _machineServices.GetMachines(pageNumber, pageSize);

            return Ok(machines);
        }

        [Authorize(Roles =
        "Admin,PlantManager,ProductionManager," +
        "ProductionPlanner,QualityInspector," +
        "MaintenanceTechnician,Operator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(int id)
        {
            var machine = await _machineServices.GetMachine(id);

            if (machine == null)
            {
                return NotFound();
            }

            return machine;
        }

        [Authorize(Roles = "Admin,PlantManager")]
        [HttpPost]
        public async Task<ActionResult<Machine>> PostMachine(MachineRequest machine)
        {
            var createdMachine = await _machineServices.CreateMachine(machine);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Create", nameof(Machine), createdMachine.MachineId);

            return CreatedAtAction(
                nameof(GetMachine),
                new { id = createdMachine.MachineId },
                createdMachine);
        }

        [Authorize(Roles = "Admin,PlantManager")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Machine>> PutMachine(int id, MachineRequest machine)
        {
            var updatedMachine = await _machineServices.UpdateMachine(id, machine);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Update", nameof(Machine), id);

            return Ok(updatedMachine);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Machine>> DeleteMachine(int id)
        {
            var deletedMachine = await _machineServices.DeleteMachine(id);
            _auditLogService.Add(User.Identity?.Name ?? string.Empty, "Delete", nameof(Machine), id);

            return Ok(deletedMachine);
        }
    }
}
