using backend.Models;

namespace BusinessLayer.Interfaces
{
    public interface IMaintenanceLogServices
    {
        public Task<List<MaintenanceLog>?> GetMaintenanceLogs();
        public Task<MaintenanceLog?> GetMaintenanceLog(int id);
        public Task<MaintenanceLog> CreateMaintenanceLog(MaintenanceLog maintenanceLog);
        public Task<MaintenanceLog?> UpdateMaintenanceLog(int id, MaintenanceLog maintenanceLog);
        public Task<MaintenanceLog?> DeleteMaintenanceLog(int id);
    }
}
