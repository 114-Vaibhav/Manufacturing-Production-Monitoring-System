using backend.Models;

namespace BusinessLayer.Interfaces
{
    public interface IProductionOrderServices
    {
        public Task<List<ProductionOrder>?> GetProductionOrder();
        public Task<ProductionOrder?> GetProductionOrder(int id);
        public Task<ProductionOrder> CreateProductionOrder(ProductionOrder product);
        public Task<ProductionOrder?> UpdateProductionOrder(int id, ProductionOrder product);
        public Task<ProductionOrder?> DeleteProductionOrder(int id);
    }
}
