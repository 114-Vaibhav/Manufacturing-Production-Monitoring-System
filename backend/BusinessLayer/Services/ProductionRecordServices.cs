using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class ProductionRecordServices : IProductionRecordServices
    {
        IRepository<int, ProductionRecord> _productionRecordRepository;

        public ProductionRecordServices(IRepository<int, ProductionRecord> productionRecordRepository)
        {
            _productionRecordRepository = productionRecordRepository;
        }

        public async Task<List<ProductionRecord>?> GetProductionRecord(int pageNumber = 1, int pageSize = 10)
        {
            return await _productionRecordRepository.GetAll(pageNumber, pageSize);
        }

        public async Task<ProductionRecord?> GetProductionRecord(int id)
        {
            return await _productionRecordRepository.Get(id);
        }

        public async Task<ProductionRecord> CreateProductionRecord(ProductionRecordRequest request)
        {
            var product = MapProductionRecord(request);
            ProductionRecordValidator.ValidateProductionRecord(product);

            var records = await _productionRecordRepository.GetAll();
            DuplicateGuard.ThrowIfDuplicate(
                records,
                item => item.ProductionPlanId == product.ProductionPlanId &&
                        item.ProducedQuantity == product.ProducedQuantity,
                nameof(ProductionRecord));

            return await _productionRecordRepository.Create(product);
        }

        public async Task<ProductionRecord?> UpdateProductionRecord(
            int id,
            ProductionRecordRequest request)
        {
            var product = MapProductionRecord(request);
            product.Id = id;
            ProductionRecordValidator.ValidateProductionRecord(product);

            return await _productionRecordRepository.Update(id, product);
        }

        public async Task<ProductionRecord?> DeleteProductionRecord(int id)
        {
            return await _productionRecordRepository.Delete(id);
        }

        private static ProductionRecord MapProductionRecord(ProductionRecordRequest request)
        {
            return new ProductionRecord
            {
                ProductionPlanId = request.ProductionPlanId,
                ProducedQuantity = request.ProducedQuantity,
                ProductionDate = DateTime.UtcNow
            };
        }
    }
}
