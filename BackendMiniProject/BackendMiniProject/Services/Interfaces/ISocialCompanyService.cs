using BackendMiniProject.Models;

namespace BackendMiniProject.Services.Interfaces
{
    public interface ISocialCompanyService
    {
        Task<IEnumerable<SocialMediaCompany>> GetAllAsync();
    }
}
