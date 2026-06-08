using backend.Models;

namespace BusinessLayer.Interfaces
{
    public interface IDefectServices
    {
        public Task<List<Defect>?> GetDefects();
        public Task<Defect?> GetDefect(int id);
        public Task<Defect> CreateDefect(Defect defect);
        public Task<Defect?> UpdateDefect(int id, Defect defect);
        public Task<Defect?> DeleteDefect(int id);
    }
}
