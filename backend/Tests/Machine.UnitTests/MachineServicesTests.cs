using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;
using Xunit;

namespace MachineUnitTests
{
    public class MachineServicesTests
    {
        [Fact]
        public async Task CreateMachine_WithValidRequest_CreatesMachine()
        {
            var repository = new FakeMachineRepository();
            var service = new MachineServices(repository);

            var createdMachine = await service.CreateMachine(CreateValidRequest());

            Assert.Equal("CNC Machine", createdMachine.MachineName);
            Assert.Equal("CNC-001", createdMachine.MachineCode);
            Assert.Single(repository.Items);
        }

        [Fact]
        public async Task CreateMachine_WithDuplicateRequest_ThrowsValidationException()
        {
            var repository = new FakeMachineRepository();
            repository.Items.Add(new Machine
            {
                MachineName = "CNC Machine",
                MachineCode = "CNC-001",
                LocationId = 1,
                Status = MachineStatus.Running
            });
            var service = new MachineServices(repository);

            await Assert.ThrowsAsync<ValidationException>(() =>
                service.CreateMachine(CreateValidRequest()));
        }

        [Fact]
        public async Task CreateMachine_WithInvalidRequest_ThrowsValidationException()
        {
            var repository = new FakeMachineRepository();
            var service = new MachineServices(repository);
            var request = CreateValidRequest();
            request.MachineName = string.Empty;

            await Assert.ThrowsAsync<ValidationException>(() =>
                service.CreateMachine(request));
        }

        [Fact]
        public async Task UpdateMachine_SetsRouteIdBeforeUpdate()
        {
            var repository = new FakeMachineRepository();
            var service = new MachineServices(repository);

            var updatedMachine = await service.UpdateMachine(12, CreateValidRequest());

            Assert.Equal(12, updatedMachine?.MachineId);
        }

        [Fact]
        public async Task DeleteMachine_RemovesMachine()
        {
            var repository = new FakeMachineRepository();
            repository.Items.Add(new Machine
            {
                MachineId = 1,
                MachineName = "CNC Machine",
                MachineCode = "CNC-001",
                LocationId = 1,
                Status = MachineStatus.Running
            });
            var service = new MachineServices(repository);

            var deletedMachine = await service.DeleteMachine(1);

            Assert.Equal(1, deletedMachine?.MachineId);
            Assert.Empty(repository.Items);
        }

        private static MachineRequest CreateValidRequest()
        {
            return new MachineRequest
            {
                MachineName = "CNC Machine",
                MachineCode = "CNC-001",
                LocationId = 1,
                Status = MachineStatus.Running
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

            public Task<List<Machine>?> GetAll(int pageNumber, int pageSize)
            {
                var skip = (pageNumber - 1) * pageSize;
                return Task.FromResult<List<Machine>?>(Items.Skip(skip).Take(pageSize).ToList());
            }

            public Task<Machine?> GetByUserName(string userName)
            {
                return Task.FromResult<Machine?>(null);
            }

            public Task<Machine?> Update(int key, Machine item)
            {
                return Task.FromResult<Machine?>(item);
            }
        }
    }
}
