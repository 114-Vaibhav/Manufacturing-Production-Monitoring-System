using backend.Models;
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

        public async Task<Product> CreateProduct(Product product)
        {
            ProductValidator.ValidateProduct(product);

            return await _productRepository.Create(product);
        }

        public async Task<Product?> UpdateProduct(int id, Product product)
        {
            product.ProductId = id;
            ProductValidator.ValidateProduct(product);

            return await _productRepository.Update(id, product);
        }

        public async Task<Product?> DeleteProduct(int id)
        {
            return await _productRepository.Delete(id);
        }
    }
}
