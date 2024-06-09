using BackendMiniProject.Models;
using BackendMiniProject.ViewModels.Courses;

namespace BackendMiniProject.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<IEnumerable<Course>> GetAllWithAllDatasAsync();
        IEnumerable<CourseVM> GetMappedDatas(IEnumerable<Course> course);
        Task<int> GetCountAsync();
        Task<bool> ExistAsync(string name);
        Task CreateAsync(Course course);
        Task<Course> GetByIdAsync(int id);
        Task DeleteAsync(Course course);
        Task UpdatedAsync();
        Task<Course> GetByIdWithCoursesImagesAsync(int id);
        Task<CourseImage> GetCourseImageByIdAsync(int id);
        Task ImgDeleteAsync(CourseImage image);
    }
}
