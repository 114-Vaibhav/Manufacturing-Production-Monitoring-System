using backend.Models;
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

        public async Task<Defect> CreateDefect(Defect defect)
        {
            // DefectValidator.ValidateDefect(defect);

            return await _defectRepository.Create(defect);
        }

        public async Task<Defect?> UpdateDefect(int id, Defect defect)
        {
            defect.DefectId = id;
            // DefectValidator.ValidateDefect(defect);

            return await _defectRepository.Update(id, defect);
        }

        public async Task<Defect?> DeleteDefect(int id)
        {
            return await _defectRepository.Delete(id);
        }
    }
}
