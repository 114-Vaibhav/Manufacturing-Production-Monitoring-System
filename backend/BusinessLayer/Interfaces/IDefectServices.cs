using backend.Models;
using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IDefectServices
    {
        public Task<List<Defect>?> GetDefects(int pageNumber = 1, int pageSize = 10);
        public Task<Defect?> GetDefect(int id);
        public Task<Defect> CreateDefect(DefectRequest defect, int reportedBy);
        public Task<Defect?> UpdateDefect(int id, DefectRequest defect, int reportedBy);
        public Task<Defect?> DeleteDefect(int id);
    }
}
