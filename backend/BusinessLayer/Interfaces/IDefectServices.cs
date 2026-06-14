using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IDefectServices
    {
        public Task<List<Defect>?> GetDefects();
        public Task<Defect?> GetDefect(int id);
        public Task<Defect> CreateDefect(DefectRequest defect, int reportedBy);
        public Task<Defect?> UpdateDefect(int id, DefectRequest defect, int reportedBy);
        public Task<Defect?> DeleteDefect(int id);
    }
}
