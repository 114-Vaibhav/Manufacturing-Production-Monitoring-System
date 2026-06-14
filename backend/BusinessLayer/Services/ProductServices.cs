using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class ProductServices : IProductServices
    {
        IRepository<int, Product> _productRepository;

        public ProductServices(IRepository<int, Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>?> GetProducts()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Product?> GetProduct(int id)
        {
            return await _productRepository.Get(id);
        }

        public async Task<Product> CreateProduct(ProductRequest request)
        {
            var product = MapProduct(request);
            ProductValidator.ValidateProduct(product);

            var products = await _productRepository.GetAll();
            DuplicateGuard.ThrowIfDuplicate(
                products,
                item => item.ProductName == product.ProductName &&
                        item.ProductCode == product.ProductCode &&
                        item.Description == product.Description &&
                        item.UnitPrice == product.UnitPrice &&
                        item.Status == product.Status,
                nameof(Product));

            return await _productRepository.Create(product);
        }

        public async Task<Product?> UpdateProduct(int id, ProductRequest request)
        {
            var product = MapProduct(request);
            product.ProductId = id;
            ProductValidator.ValidateProduct(product);

            return await _productRepository.Update(id, product);
        }

        public async Task<Product?> DeleteProduct(int id)
        {
            return await _productRepository.Delete(id);
        }

        private static Product MapProduct(ProductRequest request)
        {
            return new Product
            {
                ProductName = request.ProductName,
                ProductCode = request.ProductCode,
                Description = request.Description,
                UnitPrice = request.UnitPrice,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
