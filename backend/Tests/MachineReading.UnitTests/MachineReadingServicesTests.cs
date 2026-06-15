using backend.Models;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;
using Xunit;

namespace MachineReadingUnitTests
{
    public class MachineReadingServicesTests
    {
        [Fact]
        public async Task GetMachineReadings_WithPagination_ReturnsCorrectPage()
        {
            var repository = new FakeMachineReadingRepository();
            for (var i = 1; i <= 10; i++)
            {
                repository.Items.Add(new MachineReading
                {
                    ReadingId = i,
                    MachineId = 100 + i,
                    Temperature = 30.0 + i,
                    Vibration = 5.0 + i,
                    PowerConsumption = 50.0 + i,
                    Timestamp = DateTime.UtcNow.AddMinutes(-i)
                });
            }

            var service = new MachineReadingServices(repository);
            var pageTwo = await service.GetMachineReadings(2, 3);

            Assert.Equal(3, pageTwo?.Count);
            Assert.Equal(4, pageTwo?.First().ReadingId);
        }

        private class FakeMachineReadingRepository : IRepository<int, MachineReading>
        {
            public List<MachineReading> Items { get; } = new();

            public Task<MachineReading> Create(MachineReading item)
            {
                Items.Add(item);
                return Task.FromResult(item);
            }

            public Task<MachineReading?> Delete(int key)
            {
                var item = Items.FirstOrDefault(reading => reading.ReadingId == key);
                if (item == null)
                    throw new KeyNotFoundException($"Item with key {key} not found");

                Items.Remove(item);
                return Task.FromResult<MachineReading?>(item);
            }

            public Task<MachineReading?> Get(int key)
            {
                return Task.FromResult(Items.FirstOrDefault(reading => reading.ReadingId == key));
            }

            public Task<List<MachineReading>?> GetAll()
            {
                return Task.FromResult<List<MachineReading>?>(Items);
            }

            public Task<List<MachineReading>?> GetAll(int pageNumber, int pageSize)
            {
                var skip = (pageNumber - 1) * pageSize;
                return Task.FromResult<List<MachineReading>?>(Items.Skip(skip).Take(pageSize).ToList());
            }

            public Task<MachineReading?> GetByUserName(string userName)
            {
                return Task.FromResult<MachineReading?>(null);
            }

            public Task<MachineReading?> Update(int key, MachineReading item)
            {
                return Task.FromResult<MachineReading?>(Items.Any(reading => reading.ReadingId == key) ? item : null);
            }
        }
    }
}
