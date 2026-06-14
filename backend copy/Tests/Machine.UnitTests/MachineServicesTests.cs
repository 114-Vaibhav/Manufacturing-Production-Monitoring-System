using backend.Models;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;

namespace MachineUnitTests
{
    public class MachineServicesTests
    {
        [Fact]
        public async Task CreateMachine_WithValidMachine_CreatesMachine()
        {
            var repository = new FakeMachineRepository();
            var service = new MachineServices(repository);
            var machine = CreateValidMachine();

            var createdMachine = await service.CreateMachine(machine);

            Assert.Equal("CNC Machine", createdMachine.MachineName);
            Assert.Single(repository.Items);
        }

        [Fact]
        public async Task CreateMachine_WithInvalidMachine_ThrowsValidationException()
        {
            var repository = new FakeMachineRepository();
            var service = new MachineServices(repository);
            var machine = CreateValidMachine();
            machine.MachineName = string.Empty;

            await Assert.ThrowsAsync<ValidationException>(() =>
                service.CreateMachine(machine));
        }

        [Fact]
        public async Task UpdateMachine_SetsRouteIdBeforeUpdate()
        {
            var repository = new FakeMachineRepository();
            var service = new MachineServices(repository);
            var machine = CreateValidMachine();

            var updatedMachine = await service.UpdateMachine(12, machine);

            Assert.Equal(12, updatedMachine?.MachineId);
        }

        [Fact]
        public async Task DeleteMachine_RemovesMachine()
        {
            var repository = new FakeMachineRepository();
            var service = new MachineServices(repository);
            var machine = CreateValidMachine();
            machine.MachineId = 1;
            repository.Items.Add(machine);

            var deletedMachine = await service.DeleteMachine(1);

            Assert.Equal(1, deletedMachine?.MachineId);
            Assert.Empty(repository.Items);
        }

        private static Machine CreateValidMachine()
        {
            return new Machine
            {
                MachineName = "CNC Machine",
                MachineCode = "CNC-001",
                Location = "Line A",
                Status = "Active",
                LastMaintenanceDate = DateTime.UtcNow.AddDays(-1)
            };
        }

        private class FakeMachineRepository : IRepository<int, Machine>
        {
            public List<Machine> Items { get; } = new();

            public Task<Machine> Create(Machine item)
            {
                Items.Add(item);
                return Task.FromResult(item);
            }

            public Task<Machine?> Delete(int key)
            {
                var item = Items.FirstOrDefault(machine => machine.MachineId == key);

                if (item == null)
                    throw new KeyNotFoundException($"Item with key {key} not found");

                Items.Remove(item);
                return Task.FromResult<Machine?>(item);
            }

            public Task<Machine?> Get(int key)
            {
                return Task.FromResult(
                    Items.FirstOrDefault(machine => machine.MachineId == key));
            }

            public Task<List<Machine>?> GetAll()
            {
                return Task.FromResult<List<Machine>?>(Items);
            }

            public Task<Machine?> Update(int key, Machine item)
            {
                return Task.FromResult<Machine?>(item);
            }
        }
    }
}
