using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class ProductionOrderServices : IProductionOrderServices
    {
        IRepository<int, ProductionOrder> _productionOrderRepository;

        public ProductionOrderServices(IRepository<int, ProductionOrder> productionOrderRepository)
        {
            _productionOrderRepository = productionOrderRepository;
        }

        public async Task<List<ProductionOrder>?> GetProductionOrder(int pageNumber = 1, int pageSize = 10)
        {
            return await _productionOrderRepository.GetAll(pageNumber, pageSize);
        }

        public async Task<ProductionOrder?> GetProductionOrder(int id)
        {
            return await _productionOrderRepository.Get(id);
        }

        public async Task<ProductionOrder> CreateProductionOrder(ProductionOrderRequest request)
        {
            var product = MapProductionOrder(request);
            ProductionOrderValidator.ValidateProductionOrder(product);

            var orders = await _productionOrderRepository.GetAll();
            DuplicateGuard.ThrowIfDuplicate(
                orders,
                item => item.PlanId == product.PlanId &&
                        item.MachineId == product.MachineId &&
                        item.Quantity == product.Quantity &&
                        item.ProducedQuantity == product.ProducedQuantity &&
                        item.Status == product.Status,
                nameof(ProductionOrder));

            return await _productionOrderRepository.Create(product);
        }

        public async Task<ProductionOrder?> UpdateProductionOrder(
            int id,
            ProductionOrderRequest request)
        {
            var product = MapProductionOrder(request);
            product.OrderId = id;
            ProductionOrderValidator.ValidateProductionOrder(product);

            return await _productionOrderRepository.Update(id, product);
        }

        public async Task<ProductionOrder?> DeleteProductionOrder(int id)
        {
            return await _productionOrderRepository.Delete(id);
        }

        private static ProductionOrder MapProductionOrder(ProductionOrderRequest request)
        {
            return new ProductionOrder
            {
                PlanId = request.PlanId,
                MachineId = request.MachineId,
                Quantity = request.Quantity,
                ProducedQuantity = request.ProducedQuantity,
                Status = request.Status
            };
        }
    }
}
