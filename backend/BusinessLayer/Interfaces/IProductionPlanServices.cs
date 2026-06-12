using backend.Models;

namespace BusinessLayer.Interfaces
{
    public interface IProductionPlanServices
    {
        public Task<List<ProductionPlan>?> GetProductionPlan();
        public Task<ProductionPlan?> GetProductionPlan(int id);
        public Task<ProductionPlan> CreateProductionPlan(ProductionPlan product);
        public Task<ProductionPlan?> UpdateProductionPlan(int id, ProductionPlan product);
        public Task<ProductionPlan?> DeleteProductionPlan(int id);
    }
}
