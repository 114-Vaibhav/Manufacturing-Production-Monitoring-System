using backend.Models;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;

namespace ProductUnitTests
{
    public class ProductServicesTests
    {
        [Fact]
        public async Task CreateProduct_WithValidProduct_CreatesProduct()
        {
            var repository = new FakeProductRepository();
            var service = new ProductServices(repository);
            var product = CreateValidProduct();

            var createdProduct = await service.CreateProduct(product);

            Assert.Equal("Steel Gear", createdProduct.ProductName);
            Assert.Single(repository.Items);
        }

        [Fact]
        public async Task CreateProduct_WithInvalidProduct_ThrowsValidationException()
        {
            var repository = new FakeProductRepository();
            var service = new ProductServices(repository);
            var product = CreateValidProduct();
            product.ProductCode = string.Empty;

            await Assert.ThrowsAsync<ValidationException>(() =>
                service.CreateProduct(product));
        }

        [Fact]
        public async Task UpdateProduct_SetsRouteIdBeforeUpdate()
        {
            var repository = new FakeProductRepository();
            var service = new ProductServices(repository);
            var product = CreateValidProduct();

            var updatedProduct = await service.UpdateProduct(5, product);

            Assert.Equal(5, updatedProduct?.ProductId);
        }

        [Fact]
        public async Task DeleteProduct_RemovesProduct()
        {
            var repository = new FakeProductRepository();
            var service = new ProductServices(repository);
            var product = CreateValidProduct();
            product.ProductId = 1;
            repository.Items.Add(product);

            var deletedProduct = await service.DeleteProduct(1);

            Assert.Equal(1, deletedProduct?.ProductId);
            Assert.Empty(repository.Items);
        }

        private static Product CreateValidProduct()
        {
            return new Product
            {
                ProductName = "Steel Gear",
                ProductCode = "PRD-001",
                Description = "Standard production gear.",
                UnitPrice = 120.50m,
                Status = "Active",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
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

            public Task<Product?> Update(int key, Product item)
            {
                return Task.FromResult<Product?>(item);
            }
        }
    }
}
