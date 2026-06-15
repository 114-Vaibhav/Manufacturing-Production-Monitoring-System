using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;
using Xunit;

namespace ProductionOrderUnitTests
{
    public class ProductionOrderServicesTests
    {
        [Fact]
        public async Task GetProductionOrder_WithPageSize_ReturnsCorrectPage()
        {
            var repository = new FakeProductionOrderRepository();
            for (var i = 1; i <= 15; i++)
            {
                repository.Items.Add(new ProductionOrder
                {
                    OrderId = i,
                    PlanId = 100 + i,
                    MachineId = 200 + i,
                    Quantity = i * 10,
                    ProducedQuantity = i * 5,
                    Status = ProductionOrderStatus.Planned
                });
            }

            var service = new ProductionOrderServices(repository);
            var pageOne = await service.GetProductionOrder(pageNumber: 1, pageSize: 5);
            var pageThree = await service.GetProductionOrder(pageNumber: 3, pageSize: 5);

            Assert.Equal(5, pageOne?.Count);
            Assert.Equal(5, pageThree?.Count);
            Assert.Equal(1, pageOne?.First().OrderId);
            Assert.Equal(11, pageThree?.First().OrderId);
        }

        private class FakeProductionOrderRepository : IRepository<int, ProductionOrder>
        {
            public List<ProductionOrder> Items { get; } = new();

            public Task<ProductionOrder> Create(ProductionOrder item)
            {
                Items.Add(item);
                return Task.FromResult(item);
            }

            public Task<ProductionOrder?> Delete(int key)
            {
                var item = Items.FirstOrDefault(order => order.OrderId == key);
                if (item == null)
                {
                    throw new KeyNotFoundException($"Item with key {key} not found");
                }

                Items.Remove(item);
                return Task.FromResult<ProductionOrder?>(item);
            }

            public Task<ProductionOrder?> Get(int key)
            {
                return Task.FromResult(Items.FirstOrDefault(order => order.OrderId == key));
            }

            public Task<List<ProductionOrder>?> GetAll()
            {
                return Task.FromResult<List<ProductionOrder>?>(Items);
            }

            public Task<List<ProductionOrder>?> GetAll(int pageNumber, int pageSize)
            {
                var skip = (pageNumber - 1) * pageSize;
                return Task.FromResult<List<ProductionOrder>?>(Items.Skip(skip).Take(pageSize).ToList());
            }

            public Task<ProductionOrder?> GetByUserName(string userName)
            {
                return Task.FromResult<ProductionOrder?>(null);
            }

            public Task<ProductionOrder?> Update(int key, ProductionOrder item)
            {
                var existing = Items.FirstOrDefault(order => order.OrderId == key);
                return Task.FromResult<ProductionOrder?>(existing is null ? null : item);
            }
        }
    }
}
