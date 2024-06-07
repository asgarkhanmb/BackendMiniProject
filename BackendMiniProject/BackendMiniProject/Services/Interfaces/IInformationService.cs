using BackendMiniProject.Models;

namespace BackendMiniProject.Services.Interfaces
{
    public interface IInformationService
    {
        Task<IEnumerable<Information>> GetAllAsync();
        Task<Information> GetByIdAsync(int id);
    }
}
