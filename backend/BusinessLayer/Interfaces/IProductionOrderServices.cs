using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IProductionOrderServices
    {
        public Task<List<ProductionOrder>?> GetProductionOrder(int pageNumber = 1, int pageSize = 10);
        public Task<ProductionOrder?> GetProductionOrder(int id);
        public Task<ProductionOrder> CreateProductionOrder(ProductionOrderRequest product);
        public Task<ProductionOrder?> UpdateProductionOrder(int id, ProductionOrderRequest product);
        public Task<ProductionOrder?> DeleteProductionOrder(int id);
    }
}
