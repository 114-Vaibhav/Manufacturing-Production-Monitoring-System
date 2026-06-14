using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IMachineReadingServices
    {
        public Task<List<MachineReading>?> GetMachineReadings();
        public Task<MachineReading?> GetMachineReading(int id);
        public Task<MachineReading> CreateMachineReading(MachineReadingRequest machineReading);
        public Task<MachineReading?> UpdateMachineReading(int id, MachineReadingRequest machineReading);
        public Task<MachineReading?> DeleteMachineReading(int id);
    }
}
