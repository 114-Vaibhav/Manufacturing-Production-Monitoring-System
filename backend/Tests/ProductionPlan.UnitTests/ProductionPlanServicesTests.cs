using backend.Models;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;
using Xunit;

namespace ProductionPlanUnitTests
{
    public class ProductionPlanServicesTests
    {
        [Fact]
        public async Task GetProductionPlan_WithPagination_ReturnsCorrectPage()
        {
            var repository = new FakeProductionPlanRepository();
            for (var i = 1; i <= 12; i++)
            {
                repository.Items.Add(new ProductionPlan
                {
                    PlanId = i,
                    ProductName = $"Part {i}",
                    TargetQuantity = i * 10,
                    StartDate = DateTime.UtcNow.AddDays(-i),
                    EndDate = DateTime.UtcNow.AddDays(i),
                    Status = ProductionPlanStatus.Planned,
                    CreatedBy = 1
                });
            }

            var service = new ProductionPlanServices(repository);
            var pageTwo = await service.GetProductionPlan(2, 5);

            Assert.Equal(5, pageTwo?.Count);
            Assert.Equal(6, pageTwo?.First().PlanId);
        }

        private class FakeProductionPlanRepository : IRepository<int, ProductionPlan>
        {
            public List<ProductionPlan> Items { get; } = new();

            public Task<ProductionPlan> Create(ProductionPlan item)
            {
                Items.Add(item);
                return Task.FromResult(item);
            }

            public Task<ProductionPlan?> Delete(int key)
            {
                var item = Items.FirstOrDefault(plan => plan.PlanId == key);
                if (item == null)
                    throw new KeyNotFoundException($"Item with key {key} not found");

                Items.Remove(item);
                return Task.FromResult<ProductionPlan?>(item);
            }

            public Task<ProductionPlan?> Get(int key)
            {
                return Task.FromResult(Items.FirstOrDefault(plan => plan.PlanId == key));
            }

            public Task<List<ProductionPlan>?> GetAll()
            {
                return Task.FromResult<List<ProductionPlan>?>(Items);
            }

            public Task<List<ProductionPlan>?> GetAll(int pageNumber, int pageSize)
            {
                var skip = (pageNumber - 1) * pageSize;
                return Task.FromResult<List<ProductionPlan>?>(Items.Skip(skip).Take(pageSize).ToList());
            }

            public Task<ProductionPlan?> GetByUserName(string userName)
            {
                return Task.FromResult<ProductionPlan?>(null);
            }

            public Task<ProductionPlan?> Update(int key, ProductionPlan item)
            {
                return Task.FromResult<ProductionPlan?>(Items.Any(plan => plan.PlanId == key) ? item : null);
            }
        }
    }
}
