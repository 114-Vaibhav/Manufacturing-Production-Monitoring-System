using backend.Models;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;

namespace DefectUnitTests
{
    public class DefectServicesTests
    {
        [Fact]
        public async Task CreateDefect_WithValidDefect_CreatesDefect()
        {
            var repository = new FakeDefectRepository();
            var service = new DefectServices(repository);
            var defect = CreateValidDefect();

            var createdDefect = await service.CreateDefect(defect);

            Assert.Equal("Surface Scratch", createdDefect.DefectType);
            Assert.Single(repository.Items);
        }

        [Fact]
        public async Task CreateDefect_WithInvalidDefect_ThrowsValidationException()
        {
            var repository = new FakeDefectRepository();
            var service = new DefectServices(repository);
            var defect = CreateValidDefect();
            defect.MachineId = 0;

            await Assert.ThrowsAsync<ValidationException>(() =>
                service.CreateDefect(defect));
        }

        [Fact]
        public async Task UpdateDefect_SetsRouteIdBeforeUpdate()
        {
            var repository = new FakeDefectRepository();
            var service = new DefectServices(repository);
            var defect = CreateValidDefect();

            var updatedDefect = await service.UpdateDefect(7, defect);

            Assert.Equal(7, updatedDefect?.DefectId);
        }

        [Fact]
        public async Task DeleteDefect_RemovesDefect()
        {
            var repository = new FakeDefectRepository();
            var service = new DefectServices(repository);
            var defect = CreateValidDefect();
            defect.DefectId = 1;
            repository.Items.Add(defect);

            var deletedDefect = await service.DeleteDefect(1);

            Assert.Equal(1, deletedDefect?.DefectId);
            Assert.Empty(repository.Items);
        }

        private static Defect CreateValidDefect()
        {
            return new Defect
            {
                OrderId = 1,
                MachineId = 1,
                DefectType = "Surface Scratch",
                Severity = "Medium",
                Description = "Scratch found during quality inspection.",
                ReportedBy = 1,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
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

            public Task<Defect?> Update(int key, Defect item)
            {
                return Task.FromResult<Defect?>(item);
            }
        }
    }
}
