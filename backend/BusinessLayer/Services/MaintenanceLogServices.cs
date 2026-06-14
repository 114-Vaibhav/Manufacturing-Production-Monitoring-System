using backend.Models;
using backend.Models.DTOs;
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

        public async Task<MaintenanceLog> CreateMaintenanceLog(
            MaintenanceLogRequest request,
            int engineerId)
        {
            var maintenanceLog = MapMaintenanceLog(request, engineerId);
            MaintenanceLogValidator.ValidateMaintenanceLog(maintenanceLog);

            var logs = await _maintenanceLogRepository.GetAll();
            DuplicateGuard.ThrowIfDuplicate(
                logs,
                item => item.MachineId == maintenanceLog.MachineId &&
                        item.EngineerId == maintenanceLog.EngineerId &&
                        item.IssueDescription == maintenanceLog.IssueDescription &&
                        item.Resolution == maintenanceLog.Resolution,
                nameof(MaintenanceLog));

            return await _maintenanceLogRepository.Create(maintenanceLog);
        }

        public async Task<MaintenanceLog?> UpdateMaintenanceLog(
            int id,
            MaintenanceLogRequest request,
            int engineerId)
        {
            var maintenanceLog = MapMaintenanceLog(request, engineerId);
            maintenanceLog.LogId = id;
            MaintenanceLogValidator.ValidateMaintenanceLog(maintenanceLog);

            return await _maintenanceLogRepository.Update(id, maintenanceLog);
        }

        public async Task<MaintenanceLog?> DeleteMaintenanceLog(int id)
        {
            return await _maintenanceLogRepository.Delete(id);
        }

        private static MaintenanceLog MapMaintenanceLog(
            MaintenanceLogRequest request,
            int engineerId)
        {
            return new MaintenanceLog
            {
                MachineId = request.MachineId,
                EngineerId = engineerId,
                IssueDescription = request.IssueDescription,
                Resolution = request.Resolution,
                MaintenanceDate = DateTime.UtcNow
            };
        }
    }
}
