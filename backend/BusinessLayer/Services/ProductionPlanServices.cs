using backend.Models;
using backend.Models.DTOs;
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

        public async Task<List<ProductionPlan>?> GetProductionPlan(int pageNumber = 1, int pageSize = 10)
        {
            return await _productionPlanRepository.GetAll(pageNumber, pageSize);
        }

        public async Task<ProductionPlan?> GetProductionPlan(int id)
        {
            return await _productionPlanRepository.Get(id);
        }

        public async Task<ProductionPlan> CreateProductionPlan(
            ProductionPlanRequest request,
            int createdBy)
        {
            var product = MapProductionPlan(request, createdBy);
            ProductionPlanValidator.ValidateProductionPlan(product);

            var plans = await _productionPlanRepository.GetAll();
            DuplicateGuard.ThrowIfDuplicate(
                plans,
                item => item.ProductName == product.ProductName &&
                        item.TargetQuantity == product.TargetQuantity &&
                        item.StartDate == product.StartDate &&
                        item.EndDate == product.EndDate &&
                        item.Status == product.Status &&
                        item.CreatedBy == product.CreatedBy,
                nameof(ProductionPlan));

            return await _productionPlanRepository.Create(product);
        }

        public async Task<ProductionPlan?> UpdateProductionPlan(
            int id,
            ProductionPlanRequest request,
            int createdBy)
        {
            var product = MapProductionPlan(request, createdBy);
            product.PlanId = id;
            ProductionPlanValidator.ValidateProductionPlan(product);

            return await _productionPlanRepository.Update(id, product);
        }

        public async Task<ProductionPlan?> DeleteProductionPlan(int id)
        {
            return await _productionPlanRepository.Delete(id);
        }

        private static ProductionPlan MapProductionPlan(
            ProductionPlanRequest request,
            int createdBy)
        {
            return new ProductionPlan
            {
                ProductName = request.ProductName,
                TargetQuantity = request.TargetQuantity,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status,
                CreatedBy = createdBy
            };
        }
    }
}
