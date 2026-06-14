using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IMachineServices
    {
        public Task<List<Machine>?> GetMachines();
        public Task<Machine?> GetMachine(int id);
        public Task<Machine> CreateMachine(MachineRequest machine);
        public Task<Machine?> UpdateMachine(int id, MachineRequest machine);
        public Task<Machine?> DeleteMachine(int id);
    }
}
