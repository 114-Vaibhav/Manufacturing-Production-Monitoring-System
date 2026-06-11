using backend.Models;
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

        public MachineReadingsController(IMachineReadingServices machineReadingServices)
        {
            _machineReadingServices = machineReadingServices;
        }

        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager," +
            "ProductionPlanner,QualityInspector," +
            "MaintenanceTechnician,Operator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachineReading>>> GetMachineReadings()
        {
            var machineReadings = await _machineReadingServices.GetMachineReadings();

            return Ok(machineReadings);
        }

        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager," +
            "ProductionPlanner,QualityInspector," +
            "MaintenanceTechnician,Operator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MachineReading>> GetMachineReading(int id)
        {
            var machineReading = await _machineReadingServices.GetMachineReading(id);

            if (machineReading == null)
            {
                return NotFound();
            }

            return Ok(machineReading);
        }

        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager," +
            "MaintenanceTechnician,Operator")]
        [HttpPost]
        public async Task<ActionResult<MachineReading>> PostMachineReading(
            MachineReading machineReading)
        {
            var createdMachineReading =
                await _machineReadingServices.CreateMachineReading(machineReading);

            return CreatedAtAction(
                nameof(GetMachineReading),
                new { id = createdMachineReading.ReadingId },
                createdMachineReading);
        }

        [Authorize(Roles =
            "Admin,PlantManager,ProductionManager," +
            "MaintenanceTechnician")]
        [HttpPut("{id}")]
        public async Task<ActionResult<MachineReading>> PutMachineReading(
            int id,
            MachineReading machineReading)
        {
            var updatedMachineReading =
                await _machineReadingServices.UpdateMachineReading(id, machineReading);

            if (updatedMachineReading == null)
            {
                return NotFound();
            }

            return Ok(updatedMachineReading);
        }

        [Authorize(Roles = "Admin,PlantManager")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MachineReading>> DeleteMachineReading(int id)
        {
            var deletedMachineReading =
                await _machineReadingServices.DeleteMachineReading(id);

            if (deletedMachineReading == null)
            {
                return NotFound();
            }

            return Ok(deletedMachineReading);
        }
    }
}