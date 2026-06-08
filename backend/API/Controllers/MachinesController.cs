using backend.Models;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private IMachineServices _machineServices;

        public MachinesController(IMachineServices machineServices)
        {
            _machineServices = machineServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
        {
            var machines = await _machineServices.GetMachines();

            return Ok(machines);
        }

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

        [HttpPost]
        public async Task<ActionResult<Machine>> PostMachine(Machine machine)
        {
            var createdMachine = await _machineServices.CreateMachine(machine);

            return CreatedAtAction(
                nameof(GetMachine),
                new { id = createdMachine.MachineId },
                createdMachine);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Machine>> PutMachine(int id, Machine machine)
        {
            var updatedMachine = await _machineServices.UpdateMachine(id, machine);

            return Ok(updatedMachine);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Machine>> DeleteMachine(int id)
        {
            var deletedMachine = await _machineServices.DeleteMachine(id);

            return Ok(deletedMachine);
        }
    }
}
