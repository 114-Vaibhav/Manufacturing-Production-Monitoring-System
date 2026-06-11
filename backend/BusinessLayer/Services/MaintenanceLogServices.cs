using backend.Models;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class MaintenanceLogServices : IMaintenanceLogServices
    {
        IRepository<int, MaintenanceLog> _maintenanceLogRepository;

        public MaintenanceLogServices(IRepository<int, MaintenanceLog> maintenanceLogRepository)
        {
            _maintenanceLogRepository = maintenanceLogRepository;
        }

        public async Task<List<MaintenanceLog>?> GetMaintenanceLogs()
        {
            return await _maintenanceLogRepository.GetAll();
        }

        public async Task<MaintenanceLog?> GetMaintenanceLog(int id)
        {
            return await _maintenanceLogRepository.Get(id);
        }

        public async Task<MaintenanceLog> CreateMaintenanceLog(MaintenanceLog maintenanceLog)
        {
            MaintenanceLogValidator.ValidateMaintenanceLog(maintenanceLog);

            return await _maintenanceLogRepository.Create(maintenanceLog);
        }

        public async Task<MaintenanceLog?> UpdateMaintenanceLog(int id, MaintenanceLog maintenanceLog)
        {
            maintenanceLog.LogId = id;
            MaintenanceLogValidator.ValidateMaintenanceLog(maintenanceLog);

            return await _maintenanceLogRepository.Update(id, maintenanceLog);
        }

        public async Task<MaintenanceLog?> DeleteMaintenanceLog(int id)
        {
            return await _maintenanceLogRepository.Delete(id);
        }
    }
}
