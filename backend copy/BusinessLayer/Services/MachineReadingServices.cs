using backend.Models;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class MachineReadingServices : IMachineReadingServices
    {
        IRepository<int, MachineReading> _machineReadingRepository;

        public MachineReadingServices(IRepository<int, MachineReading> machineReadingRepository)
        {
            _machineReadingRepository = machineReadingRepository;
        }

        public async Task<List<MachineReading>?> GetMachineReadings()
        {
            return await _machineReadingRepository.GetAll();
        }

        public async Task<MachineReading?> GetMachineReading(int id)
        {
            return await _machineReadingRepository.Get(id);
        }

        public async Task<MachineReading> CreateMachineReading(MachineReading machineReading)
        {
            // MachineReadingValidator.ValidateMachineReading(machineReading);

            return await _machineReadingRepository.Create(machineReading);
        }

        public async Task<MachineReading?> UpdateMachineReading(int id, MachineReading machineReading)
        {
            machineReading.MachineId = id;
            // MachineReadingValidator.ValidateMachineReading(machineReading);

            return await _machineReadingRepository.Update(id, machineReading);
        }

        public async Task<MachineReading?> DeleteMachineReading(int id)
        {
            return await _machineReadingRepository.Delete(id);
        }
    }
}
