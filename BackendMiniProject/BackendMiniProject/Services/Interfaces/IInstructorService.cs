using BackendMiniProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackendMiniProject.Services.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<Instructor>> GetAllAsync();
        Task CreateAsync(Instructor instructor);
        Task DeleteAsync(Instructor instructor);
        Task EditAsync();
        Task<bool> ExistEmailAsync(string email);
        Task<Instructor> GetByIdAsync(int id);
        Task<Instructor> GetByIdWithSocialAsync(int id);
        Task<bool> ExistExceptByIdAsync(int id, string email);
        Task<SelectList> GetAllSelectedAsync();
    }
}
