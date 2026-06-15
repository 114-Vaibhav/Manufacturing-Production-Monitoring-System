using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class MachineServices : IMachineServices
    {
        IRepository<int, Machine> _machineRepository;

        public MachineServices(IRepository<int, Machine> machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<List<Machine>?> GetMachines(int pageNumber = 1, int pageSize = 10)
        {
            return await _machineRepository.GetAll(pageNumber, pageSize);
        }

        public async Task<Machine?> GetMachine(int id)
        {
            return await _machineRepository.Get(id);
        }

        public async Task<Machine> CreateMachine(MachineRequest request)
        {
            var machine = MapMachine(request);
            MachineValidator.ValidateMachine(machine);

            var machines = await _machineRepository.GetAll();
            DuplicateGuard.ThrowIfDuplicate(
                machines,
                item => item.MachineName == machine.MachineName &&
                        item.MachineCode == machine.MachineCode &&
                        item.LocationId == machine.LocationId &&
                        item.Status == machine.Status &&
                        item.LastMaintenanceDate == machine.LastMaintenanceDate,
                nameof(Machine));

            return await _machineRepository.Create(machine);
        }

        public async Task<Machine?> UpdateMachine(int id, MachineRequest request)
        {
            var machine = MapMachine(request);
            machine.MachineId = id;
            MachineValidator.ValidateMachine(machine);

            return await _machineRepository.Update(id, machine);
        }

        public async Task<Machine?> DeleteMachine(int id)
        {
            return await _machineRepository.Delete(id);
        }

        private static Machine MapMachine(MachineRequest request)
        {
            return new Machine
            {
                MachineName = request.MachineName,
                MachineCode = request.MachineCode,
                LocationId = request.LocationId,
                Status = request.Status,
                LastMaintenanceDate = request.LastMaintenanceDate
            };
        }
    }
}
