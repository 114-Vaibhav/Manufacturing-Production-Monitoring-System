using backend.Models;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class ProductionPlanServices : IProductionPlanServices
    {
        IRepository<int, ProductionPlan> _productionPlanRepository;

        public ProductionPlanServices(IRepository<int, ProductionPlan> productionPlanRepository)
        {
            _productionPlanRepository = productionPlanRepository;
        }

        public async Task<List<ProductionPlan>?> GetProductionPlan()
        {
            return await _productionPlanRepository.GetAll();
        }

        public async Task<ProductionPlan?> GetProductionPlan(int id)
        {
            return await _productionPlanRepository.Get(id);
        }

        public async Task<ProductionPlan> CreateProductionPlan(ProductionPlan product)
        {
            // ProductionPlanValidator.ValidateProductionPlan(product);

            return await _productionPlanRepository.Create(product);
        }

        public async Task<ProductionPlan?> UpdateProductionPlan(int id, ProductionPlan product)
        {
            product.PlanId = id;
            // ProductionPlanValidator.ValidateProductionPlan(product);

            return await _productionPlanRepository.Update(id, product);
        }

        public async Task<ProductionPlan?> DeleteProductionPlan(int id)
        {
            return await _productionPlanRepository.Delete(id);
        }
    }
}
