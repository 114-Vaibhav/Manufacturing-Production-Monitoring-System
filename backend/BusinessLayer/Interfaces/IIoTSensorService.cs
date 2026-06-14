using backend.Models;
namespace BusinessLayer.Interfaces
{
    public interface IIoTSensorService
    {
        Task GenerateMachineReadingsAsync();
    }
}