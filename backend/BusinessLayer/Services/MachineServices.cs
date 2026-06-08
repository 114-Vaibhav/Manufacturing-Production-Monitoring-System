// using backend.Models;
// using BusinessLayer.Interfaces;
// using DataAccessLayer.Interfaces;

// namespace BusinessLayer.Services
// {
//     public class MachineServices : IMachineServices
//     {
//         IRepository<int, Machine> _machineRepository;

//         public MachineServices(IRepository<int, Machine> machineRepository)
//         {
//             _machineRepository = machineRepository;
//         }

//         public async Task<List<Machine>?> GetMachines()
//         {
//             return await _machineRepository.GetAll();
//         }

//         public async Task<Machine?> GetMachine(int id)
//         {
//             return await _machineRepository.Get(id);
//         }

//         public async Task<Machine> CreateMachine(Machine machine)
//         {
//             MachineValidator.ValidateMachine(machine);

//             return await _machineRepository.Create(machine);
//         }

//         public async Task<Machine?> UpdateMachine(int id, Machine machine)
//         {
//             machine.MachineId = id;
//             MachineValidator.ValidateMachine(machine);

//             return await _machineRepository.Update(id, machine);
//         }

//         public async Task<Machine?> DeleteMachine(int id)
//         {
//             return await _machineRepository.Delete(id);
//         }
//     }
// }
