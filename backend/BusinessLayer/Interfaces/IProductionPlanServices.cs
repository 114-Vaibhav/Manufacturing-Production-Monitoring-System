using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IProductionPlanServices
    {
        public Task<List<ProductionPlan>?> GetProductionPlan(int pageNumber = 1, int pageSize = 10);
        public Task<ProductionPlan?> GetProductionPlan(int id);
        public Task<ProductionPlan> CreateProductionPlan(ProductionPlanRequest product, int createdBy);
        public Task<ProductionPlan?> UpdateProductionPlan(int id, ProductionPlanRequest product, int createdBy);
        public Task<ProductionPlan?> DeleteProductionPlan(int id);
    }
}
