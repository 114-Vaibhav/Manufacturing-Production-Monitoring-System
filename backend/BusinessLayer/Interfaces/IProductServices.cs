using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IProductServices
    {
        public Task<List<Product>?> GetProducts();
        public Task<Product?> GetProduct(int id);
        public Task<Product> CreateProduct(ProductRequest product);
        public Task<Product?> UpdateProduct(int id, ProductRequest product);
        public Task<Product?> DeleteProduct(int id);
    }
}
