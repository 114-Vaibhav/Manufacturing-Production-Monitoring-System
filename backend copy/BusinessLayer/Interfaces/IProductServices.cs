using backend.Models;

namespace BusinessLayer.Interfaces
{
    public interface IProductServices
    {
        public Task<List<Product>?> GetProducts();
        public Task<Product?> GetProduct(int id);
        public Task<Product> CreateProduct(Product product);
        public Task<Product?> UpdateProduct(int id, Product product);
        public Task<Product?> DeleteProduct(int id);
    }
}
