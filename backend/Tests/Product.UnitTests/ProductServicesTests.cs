using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;
using Xunit;

namespace ProductUnitTests
{
    public class ProductServicesTests
    {
        [Fact]
        public async Task CreateProduct_WithValidRequest_AddsCurrentTime()
        {
            var repository = new FakeProductRepository();
            var service = new ProductServices(repository);

            var createdProduct = await service.CreateProduct(CreateValidRequest());

            Assert.Equal("Steel Gear", createdProduct.ProductName);
            Assert.True(createdProduct.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
            Assert.Single(repository.Items);
        }

        [Fact]
        public async Task CreateProduct_WithDuplicateRequest_ThrowsValidationException()
        {
            var repository = new FakeProductRepository();
            repository.Items.Add(new Product
            {
                ProductName = "Steel Gear",
                ProductCode = "PRD-001",
                Description = "Standard production gear.",
                UnitPrice = 120.50m,
                Status = "Active",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            });
            var service = new ProductServices(repository);

            await Assert.ThrowsAsync<ValidationException>(() =>
                service.CreateProduct(CreateValidRequest()));
        }

        [Fact]
        public async Task CreateProduct_WithInvalidRequest_ThrowsValidationException()
        {
            var repository = new FakeProductRepository();
            var service = new ProductServices(repository);
            var request = CreateValidRequest();
            request.ProductCode = string.Empty;

            await Assert.ThrowsAsync<ValidationException>(() =>
                service.CreateProduct(request));
        }

        [Fact]
        public async Task UpdateProduct_SetsRouteIdBeforeUpdate()
        {
            var repository = new FakeProductRepository();
            var service = new ProductServices(repository);

            var updatedProduct = await service.UpdateProduct(5, CreateValidRequest());

            Assert.Equal(5, updatedProduct?.ProductId);
        }

        [Fact]
        public async Task DeleteProduct_RemovesProduct()
        {
            var repository = new FakeProductRepository();
            repository.Items.Add(new Product
            {
                ProductId = 1,
                ProductName = "Steel Gear",
                ProductCode = "PRD-001",
                Description = "Standard production gear.",
                UnitPrice = 120.50m,
                Status = "Active"
            });
            var service = new ProductServices(repository);

            var deletedProduct = await service.DeleteProduct(1);

            Assert.Equal(1, deletedProduct?.ProductId);
            Assert.Empty(repository.Items);
        }

        private static ProductRequest CreateValidRequest()
        {
            return new ProductRequest
            {
                ProductName = "Steel Gear",
                ProductCode = "PRD-001",
                Description = "Standard production gear.",
                UnitPrice = 120.50m,
                Status = "Active"
            };
        }

        private class FakeProductRepository : IRepository<int, Product>
        {
            public List<Product> Items { get; } = new();

            public Task<Product> Create(Product item)
            {
                Items.Add(item);
                return Task.FromResult(item);
            }

            public Task<Product?> Delete(int key)
            {
                var item = Items.FirstOrDefault(product => product.ProductId == key);

                if (item == null)
                    throw new KeyNotFoundException($"Item with key {key} not found");

                Items.Remove(item);
                return Task.FromResult<Product?>(item);
            }

            public Task<Product?> Get(int key)
            {
                return Task.FromResult(
                    Items.FirstOrDefault(product => product.ProductId == key));
            }

            public Task<List<Product>?> GetAll()
            {
                return Task.FromResult<List<Product>?>(Items);
            }

            public Task<List<Product>?> GetAll(int pageNumber, int pageSize)
            {
                var skip = (pageNumber - 1) * pageSize;
                return Task.FromResult<List<Product>?>(Items.Skip(skip).Take(pageSize).ToList());
            }

            public Task<Product?> GetByUserName(string userName)
            {
                return Task.FromResult<Product?>(null);
            }

            public Task<Product?> Update(int key, Product item)
            {
                return Task.FromResult<Product?>(item);
            }
        }
    }
}
