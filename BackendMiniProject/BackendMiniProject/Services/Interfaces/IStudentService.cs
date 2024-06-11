using BackendMiniProject.Models;
using BackendMiniProject.ViewModels.Students;

namespace BackendMiniProject.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentVM>> GetAllAsync(int? take = null);
        Task<Student> GetByIdAsync(int id);
        Task<bool> ExistAsync(string fullname);
        Task CreateAsync(Student student);
        Task DeleteAsync(Student student);
        Task EditAsync();
    }
}
