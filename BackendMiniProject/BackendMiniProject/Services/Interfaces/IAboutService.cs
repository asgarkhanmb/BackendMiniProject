using BackendMiniProject.Models;

namespace BackendMiniProject.Services.Interfaces
{
    public interface IAboutService
    {
        Task<IEnumerable<About>> GetAllAsync();
        Task<About> GetByIdAsync(int id);
        Task<About> GetAboutAsync();

    }
}
