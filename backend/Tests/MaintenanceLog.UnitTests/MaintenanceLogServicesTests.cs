using backend.Models;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;
using Xunit;

namespace MaintenanceLogUnitTests
{
    public class MaintenanceLogServicesTests
    {
        [Fact]
        public async Task GetMaintenanceLogs_WithPagination_ReturnsCorrectPage()
        {
            var repository = new FakeMaintenanceLogRepository();
            for (var i = 1; i <= 9; i++)
            {
                repository.Items.Add(new MaintenanceLog
                {
                    LogId = i,
                    MachineId = 10 + i,
                    EngineerId = 20 + i,
                    IssueDescription = $"Issue {i}",
                    Resolution = $"Resolved {i}",
                    MaintenanceDate = DateTime.UtcNow.AddDays(-i)
                });
            }

            var service = new MaintenanceLogServices(repository);
            var pageTwo = await service.GetMaintenanceLogs(2, 4);

            Assert.Equal(4, pageTwo?.Count);
            Assert.Equal(5, pageTwo?.First().LogId);
        }

        private class FakeMaintenanceLogRepository : IRepository<int, MaintenanceLog>
        {
            public List<MaintenanceLog> Items { get; } = new();

            public Task<MaintenanceLog> Create(MaintenanceLog item)
            {
                Items.Add(item);
                return Task.FromResult(item);
            }

            public Task<MaintenanceLog?> Delete(int key)
            {
                var item = Items.FirstOrDefault(log => log.LogId == key);
                if (item == null)
                    throw new KeyNotFoundException($"Item with key {key} not found");

                Items.Remove(item);
                return Task.FromResult<MaintenanceLog?>(item);
            }

            public Task<MaintenanceLog?> Get(int key)
            {
                return Task.FromResult(Items.FirstOrDefault(log => log.LogId == key));
            }

            public Task<List<MaintenanceLog>?> GetAll()
            {
                return Task.FromResult<List<MaintenanceLog>?>(Items);
            }

            public Task<List<MaintenanceLog>?> GetAll(int pageNumber, int pageSize)
            {
                var skip = (pageNumber - 1) * pageSize;
                return Task.FromResult<List<MaintenanceLog>?>(Items.Skip(skip).Take(pageSize).ToList());
            }

            public Task<MaintenanceLog?> GetByUserName(string userName)
            {
                return Task.FromResult<MaintenanceLog?>(null);
            }

            public Task<MaintenanceLog?> Update(int key, MaintenanceLog item)
            {
                return Task.FromResult<MaintenanceLog?>(Items.Any(log => log.LogId == key) ? item : null);
            }
        }
    }
}
