using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IProductionRecordServices
    {
        public Task<List<ProductionRecord>?> GetProductionRecord();
        public Task<ProductionRecord?> GetProductionRecord(int id);
        public Task<ProductionRecord> CreateProductionRecord(ProductionRecordRequest product);
        public Task<ProductionRecord?> UpdateProductionRecord(int id, ProductionRecordRequest product);
        public Task<ProductionRecord?> DeleteProductionRecord(int id);
    }
}
