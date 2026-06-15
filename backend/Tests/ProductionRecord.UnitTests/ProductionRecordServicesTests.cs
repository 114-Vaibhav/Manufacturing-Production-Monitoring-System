using backend.Models;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;
using Xunit;

namespace ProductionRecordUnitTests
{
    public class ProductionRecordServicesTests
    {
        [Fact]
        public async Task GetProductionRecord_WithPagination_ReturnsCorrectPage()
        {
            var repository = new FakeProductionRecordRepository();
            for (var i = 1; i <= 11; i++)
            {
                repository.Items.Add(new ProductionRecord
                {
                    Id = i,
                    ProductionPlanId = 100 + i,
                    ProducedQuantity = i * 2,
                    ProductionDate = DateTime.UtcNow.AddDays(-i)
                });
            }

            var service = new ProductionRecordServices(repository);
            var pageThree = await service.GetProductionRecord(3, 4);

            Assert.Equal(3, pageThree?.Count);
            Assert.Equal(9, pageThree?.First().Id);
        }

        private class FakeProductionRecordRepository : IRepository<int, ProductionRecord>
        {
            public List<ProductionRecord> Items { get; } = new();

            public Task<ProductionRecord> Create(ProductionRecord item)
            {
                Items.Add(item);
                return Task.FromResult(item);
            }

            public Task<ProductionRecord?> Delete(int key)
            {
                var item = Items.FirstOrDefault(record => record.Id == key);
                if (item == null)
                    throw new KeyNotFoundException($"Item with key {key} not found");

                Items.Remove(item);
                return Task.FromResult<ProductionRecord?>(item);
            }

            public Task<ProductionRecord?> Get(int key)
            {
                return Task.FromResult(Items.FirstOrDefault(record => record.Id == key));
            }

            public Task<List<ProductionRecord>?> GetAll()
            {
                return Task.FromResult<List<ProductionRecord>?>(Items);
            }

            public Task<List<ProductionRecord>?> GetAll(int pageNumber, int pageSize)
            {
                var skip = (pageNumber - 1) * pageSize;
                return Task.FromResult<List<ProductionRecord>?>(Items.Skip(skip).Take(pageSize).ToList());
            }

            public Task<ProductionRecord?> GetByUserName(string userName)
            {
                return Task.FromResult<ProductionRecord?>(null);
            }

            public Task<ProductionRecord?> Update(int key, ProductionRecord item)
            {
                return Task.FromResult<ProductionRecord?>(Items.Any(record => record.Id == key) ? item : null);
            }
        }
    }
}
