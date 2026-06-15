using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IMachineReadingServices
    {
        public Task<List<MachineReading>?> GetMachineReadings(int pageNumber = 1, int pageSize = 10);
        public Task<MachineReading?> GetMachineReading(int id);
        public Task<MachineReading> CreateMachineReading(MachineReadingRequest machineReading);
        public Task<MachineReading?> UpdateMachineReading(int id, MachineReadingRequest machineReading);
        public Task<MachineReading?> DeleteMachineReading(int id);
    }
}
