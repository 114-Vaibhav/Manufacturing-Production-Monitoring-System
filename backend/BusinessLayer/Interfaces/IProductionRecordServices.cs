using backend.Models;

namespace BusinessLayer.Interfaces
{
    public interface IProductionRecordServices
    {
        public Task<List<ProductionRecord>?> GetProductionRecord();
        public Task<ProductionRecord?> GetProductionRecord(int id);
        public Task<ProductionRecord> CreateProductionRecord(ProductionRecord product);
        public Task<ProductionRecord?> UpdateProductionRecord(int id, ProductionRecord product);
        public Task<ProductionRecord?> DeleteProductionRecord(int id);
    }
}
