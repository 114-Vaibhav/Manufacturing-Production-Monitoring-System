using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IMaintenanceLogServices
    {
        public Task<List<MaintenanceLog>?> GetMaintenanceLogs(int pageNumber = 1, int pageSize = 10);
        public Task<MaintenanceLog?> GetMaintenanceLog(int id);
        public Task<MaintenanceLog> CreateMaintenanceLog(MaintenanceLogRequest maintenanceLog, int engineerId);
        public Task<MaintenanceLog?> UpdateMaintenanceLog(int id, MaintenanceLogRequest maintenanceLog, int engineerId);
        public Task<MaintenanceLog?> DeleteMaintenanceLog(int id);
    }
}
