using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;
using Xunit;

namespace DefectUnitTests
{
    public class DefectServicesTests
    {
        [Fact]
        public async Task CreateDefect_WithValidRequest_AddsCurrentUserAndTime()
        {
            var repository = new FakeDefectRepository();
            var service = new DefectServices(repository);

            var createdDefect = await service.CreateDefect(CreateValidRequest(), reportedBy: 4);

            Assert.Equal(4, createdDefect.ReportedBy);
            Assert.True(createdDefect.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
            Assert.Single(repository.Items);
        }

        [Fact]
        public async Task CreateDefect_WithDuplicateRequest_ThrowsValidationException()
        {
            var repository = new FakeDefectRepository();
            repository.Items.Add(new Defect
            {
                OrderId = 1,
                MachineId = 2,
                Type = DefectType.Quality,
                Severity = DefectSeverity.Medium,
                Description = "Scratch found during inspection.",
                ReportedBy = 4,
                CreatedAt = DateTime.UtcNow.AddHours(-1)
            });
            var service = new DefectServices(repository);

            await Assert.ThrowsAsync<ValidationException>(() =>
                service.CreateDefect(CreateValidRequest(), reportedBy: 4));
        }

        [Fact]
        public async Task CreateDefect_WithInvalidRequest_ThrowsValidationException()
        {
            var repository = new FakeDefectRepository();
            var service = new DefectServices(repository);
            var request = CreateValidRequest();
            request.MachineId = 0;

            await Assert.ThrowsAsync<ValidationException>(() =>
                service.CreateDefect(request, reportedBy: 4));
        }

        [Fact]
        public async Task UpdateDefect_SetsRouteIdBeforeUpdate()
        {
            var repository = new FakeDefectRepository();
            var service = new DefectServices(repository);

            var updatedDefect =
                await service.UpdateDefect(7, CreateValidRequest(), reportedBy: 4);

            Assert.Equal(7, updatedDefect?.DefectId);
        }

        [Fact]
        public async Task DeleteDefect_RemovesDefect()
        {
            var repository = new FakeDefectRepository();
            repository.Items.Add(new Defect
            {
                DefectId = 1,
                OrderId = 1,
                MachineId = 2,
                Type = DefectType.Quality,
                Severity = DefectSeverity.Medium,
                Description = "Scratch found during inspection.",
                ReportedBy = 4
            });
            var service = new DefectServices(repository);

            var deletedDefect = await service.DeleteDefect(1);

            Assert.Equal(1, deletedDefect?.DefectId);
            Assert.Empty(repository.Items);
        }

        private static DefectRequest CreateValidRequest()
        {
            return new DefectRequest
            {
                OrderId = 1,
                MachineId = 2,
                Type = DefectType.Quality,
                Severity = DefectSeverity.Medium,
                Description = "Scratch found during inspection."
            };
        }

        private class FakeDefectRepository : IRepository<int, Defect>
        {
            public List<Defect> Items { get; } = new();

            public Task<Defect> Create(Defect item)
            {
                Items.Add(item);
                return Task.FromResult(item);
            }

            public Task<Defect?> Delete(int key)
            {
                var item = Items.FirstOrDefault(defect => defect.DefectId == key);

                if (item == null)
                    throw new KeyNotFoundException($"Item with key {key} not found");

                Items.Remove(item);
                return Task.FromResult<Defect?>(item);
            }

            public Task<Defect?> Get(int key)
            {
                return Task.FromResult(
                    Items.FirstOrDefault(defect => defect.DefectId == key));
            }

            public Task<List<Defect>?> GetAll()
            {
                return Task.FromResult<List<Defect>?>(Items);
            }

            public Task<Defect?> GetByUserName(string userName)
            {
                return Task.FromResult<Defect?>(null);
            }

            public Task<Defect?> Update(int key, Defect item)
            {
                return Task.FromResult<Defect?>(item);
            }
        }
    }
}
