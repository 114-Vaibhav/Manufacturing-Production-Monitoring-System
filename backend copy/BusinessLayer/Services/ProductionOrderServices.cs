using backend.Models;
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

        public async Task<List<ProductionOrder>?> GetProductionOrder()
        {
            return await _productionOrderRepository.GetAll();
        }

        public async Task<ProductionOrder?> GetProductionOrder(int id)
        {
            return await _productionOrderRepository.Get(id);
        }

        public async Task<ProductionOrder> CreateProductionOrder(ProductionOrder product)
        {
            // ProductionOrderValidator.ValidateProductionOrder(product);

            return await _productionOrderRepository.Create(product);
        }

        public async Task<ProductionOrder?> UpdateProductionOrder(int id, ProductionOrder product)
        {
            product.OrderId = id;
            // ProductionOrderValidator.ValidateProductionOrder(product);

            return await _productionOrderRepository.Update(id, product);
        }

        public async Task<ProductionOrder?> DeleteProductionOrder(int id)
        {
            return await _productionOrderRepository.Delete(id);
        }
    }
}
