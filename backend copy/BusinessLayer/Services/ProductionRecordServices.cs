using backend.Models;
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

        public async Task<List<ProductionRecord>?> GetProductionRecord()
        {
            return await _productionRecordRepository.GetAll();
        }

        public async Task<ProductionRecord?> GetProductionRecord(int id)
        {
            return await _productionRecordRepository.Get(id);
        }

        public async Task<ProductionRecord> CreateProductionRecord(ProductionRecord product)
        {
            // ProductionRecordValidator.ValidateProductionRecord(product);

            return await _productionRecordRepository.Create(product);
        }

        public async Task<ProductionRecord?> UpdateProductionRecord(int id, ProductionRecord product)
        {
            product.Id = id;
            // ProductionRecordValidator.ValidateProductionRecord(product);

            return await _productionRecordRepository.Update(id, product);
        }

        public async Task<ProductionRecord?> DeleteProductionRecord(int id)
        {
            return await _productionRecordRepository.Delete(id);
        }
    }
}
