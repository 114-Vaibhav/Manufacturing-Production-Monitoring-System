using backend.Models;
using backend.Models.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class DefectServices : IDefectServices
    {
        IRepository<int, Defect> _defectRepository;

        public DefectServices(IRepository<int, Defect> defectRepository)
        {
            _defectRepository = defectRepository;
        }

        public async Task<List<Defect>?> GetDefects()
        {
            return await _defectRepository.GetAll();
        }

        public async Task<Defect?> GetDefect(int id)
        {
            return await _defectRepository.Get(id);
        }

        public async Task<Defect> CreateDefect(DefectRequest request, int reportedBy)
        {
            var defect = MapDefect(request, reportedBy);
            DefectValidator.ValidateDefect(defect);

            var defects = await _defectRepository.GetAll();
            DuplicateGuard.ThrowIfDuplicate(
                defects,
                item => item.OrderId == defect.OrderId &&
                        item.MachineId == defect.MachineId &&
                        item.Type == defect.Type &&
                        item.Severity == defect.Severity &&
                        item.Description == defect.Description &&
                        item.ReportedBy == defect.ReportedBy,
                nameof(Defect));

            return await _defectRepository.Create(defect);
        }

        public async Task<Defect?> UpdateDefect(int id, DefectRequest request, int reportedBy)
        {
            var defect = MapDefect(request, reportedBy);
            defect.DefectId = id;
            DefectValidator.ValidateDefect(defect);

            return await _defectRepository.Update(id, defect);
        }

        public async Task<Defect?> DeleteDefect(int id)
        {
            return await _defectRepository.Delete(id);
        }

        private static Defect MapDefect(DefectRequest request, int reportedBy)
        {
            return new Defect
            {
                OrderId = request.OrderId,
                MachineId = request.MachineId,
                Type = request.Type,
                Severity = request.Severity,
                Description = request.Description,
                ReportedBy = reportedBy,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
