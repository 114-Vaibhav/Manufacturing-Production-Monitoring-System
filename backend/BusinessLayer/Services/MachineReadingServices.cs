using backend.Models;
using backend.Models.DTOs;
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

        public async Task<List<MachineReading>?> GetMachineReadings(int pageNumber = 1, int pageSize = 10)
        {
            return await _machineReadingRepository.GetAll(pageNumber, pageSize);
        }

        public async Task<MachineReading?> GetMachineReading(int id)
        {
            return await _machineReadingRepository.Get(id);
        }

        public async Task<MachineReading> CreateMachineReading(MachineReadingRequest request)
        {
            var machineReading = MapMachineReading(request);
            MachineReadingValidator.ValidateMachineReading(machineReading);

            var readings = await _machineReadingRepository.GetAll();
            DuplicateGuard.ThrowIfDuplicate(
                readings,
                item => item.MachineId == machineReading.MachineId &&
                        item.Temperature == machineReading.Temperature &&
                        item.Vibration == machineReading.Vibration &&
                        item.PowerConsumption == machineReading.PowerConsumption,
                nameof(MachineReading));

            return await _machineReadingRepository.Create(machineReading);
        }

        public async Task<MachineReading?> UpdateMachineReading(int id, MachineReadingRequest request)
        {
            var machineReading = MapMachineReading(request);
            machineReading.ReadingId = id;
            MachineReadingValidator.ValidateMachineReading(machineReading);

            return await _machineReadingRepository.Update(id, machineReading);
        }

        public async Task<MachineReading?> DeleteMachineReading(int id)
        {
            return await _machineReadingRepository.Delete(id);
        }

        private static MachineReading MapMachineReading(MachineReadingRequest request)
        {
            return new MachineReading
            {
                MachineId = request.MachineId,
                Temperature = request.Temperature,
                Vibration = request.Vibration,
                PowerConsumption = request.PowerConsumption,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
