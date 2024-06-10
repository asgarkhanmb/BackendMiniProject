using BackendMiniProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackendMiniProject.Services.Interfaces
{
    public interface ISocialService
    {
        Task<SelectList> GetAllSelectedAsync();
        Task<IEnumerable<Social>> GetAllAsync();
        Task<Social> GetByIdAsync(int id);
        Task<bool> ExistAsync(string name);
        Task CreateAsync(Social social);
        Task DeleteAsync(Social social);
        Task EditAsync();
    }
}
