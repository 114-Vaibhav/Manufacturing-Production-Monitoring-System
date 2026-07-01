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
            // 1. Fetch the existing entity so EF Core tracks THIS specific instance
            var existingReading = await _machineReadingRepository.Get(id);
            
            if (existingReading == null)
            {
                return null; // Handle not found (Controller should return 404)
            }

            // 2. Map the updated values onto the EXISTING tracked entity
            existingReading.MachineId = request.MachineId;
            existingReading.Temperature = request.Temperature;
            existingReading.Vibration = request.Vibration;
            existingReading.PowerConsumption = request.PowerConsumption;
            existingReading.Timestamp = DateTime.UtcNow; 

            // 3. Validate the updated entity
            MachineReadingValidator.ValidateMachineReading(existingReading);

            // 4. Pass the tracked entity to the repository. 
            // Since it's the same instance EF Core loaded in step 1, it will save successfully.
            return await _machineReadingRepository.Update(id, existingReading);
        }
        // public async Task<MachineReading?> UpdateMachineReading(int id, MachineReadingRequest request)
        // {
        //     var machineReading = MapMachineReading(request);
        //     machineReading.ReadingId = id;
        //     MachineReadingValidator.ValidateMachineReading(machineReading);

        //     return await _machineReadingRepository.Update(id, machineReading);
        // }

        public async Task<MachineReading?> DeleteMachineReading(int id)
        {
            return await _machineReadingRepository.Delete(id);
        }

        private static MachineReading MapMachineReading(MachineReadingRequest request)
        {
            return new MachineReading
            {
                ReadingId = request.ReadingId,
                MachineId = request.MachineId,
                Temperature = request.Temperature,
                Vibration = request.Vibration,
                PowerConsumption = request.PowerConsumption,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
