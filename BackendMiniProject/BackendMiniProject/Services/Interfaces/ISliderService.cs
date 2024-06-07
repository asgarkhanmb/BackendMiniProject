using BackendMiniProject.Models;
using BackendMiniProject.ViewModels.Sliders;

namespace BackendMiniProject.Services.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<Slider>> GetAllAsync();
        Task<Slider> GetByIdAsync(int id);
    }
}
