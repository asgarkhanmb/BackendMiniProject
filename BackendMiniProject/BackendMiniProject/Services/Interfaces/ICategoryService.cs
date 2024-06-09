using BackendMiniProject.Models;
using BackendMiniProject.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackendMiniProject.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetAllWithAllDatasAsync();
        IEnumerable<CategoryCourseVM> GetMappedDatas(IEnumerable<Category> category);
        Task<int> GetCountAsync();
        Task<bool> ExistAsync(string name);
        Task CreateAsync(Category category);
        Task<Category> GetByIdAsync(int id);
        Task DeleteAsync(Category category);
        Task<SelectList> GetAllSelectedAsync();
        Task<Category> GetByIdWithCoursesAsync(int id);
        Task<bool> ExistExceptByIdAsync(int id, string name);
        Task UpdatedAsync();
    }
}
