using backend.Models;

namespace BusinessLayer.Interfaces
{
    public interface IMachineServices
    {
        public Task<List<Machine>?> GetMachines();
        public Task<Machine?> GetMachine(int id);
        public Task<Machine> CreateMachine(Machine machine);
        public Task<Machine?> UpdateMachine(int id, Machine machine);
        public Task<Machine?> DeleteMachine(int id);
    }
}
