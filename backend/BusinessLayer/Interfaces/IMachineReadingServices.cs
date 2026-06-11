using backend.Models;

namespace BusinessLayer.Interfaces
{
    public interface IMachineReadingServices
    {
        public Task<List<MachineReading>?> GetMachineReadings();
        public Task<MachineReading?> GetMachineReading(int id);
        public Task<MachineReading> CreateMachineReading(MachineReading machineReading);
        public Task<MachineReading?> UpdateMachineReading(int id, MachineReading machineReading);
        public Task<MachineReading?> DeleteMachineReading(int id);
    }
}
