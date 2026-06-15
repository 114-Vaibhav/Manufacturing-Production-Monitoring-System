using backend.Models;

// namespace BusinessLayer.Interfaces
// {
//     public interface IProductionAnalyticsServices
//     {
//         public Task<List<ProductionAnalytics>?> GetProductionAnalytics();
    
//     }
// }
namespace BusinessLayer.Interfaces
{
    public interface IProductionAnalyticsServices
    {
        Task<IEnumerable<ProductionAnalytics>> GetProductionAnalytics(int pageNumber = 1, int pageSize = 10);
        
        // This is the new internal method your background worker calls
        Task UpdateProductionAnalyticsInternalAsync();
    }
}