using BackendMiniProject.Data;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Services
{
    public class SocialCompanyService : ISocialCompanyService
    {
        private readonly AppDbContext _context;
        public SocialCompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SocialMediaCompany>> GetAllAsync()
        {
            return await _context.SocialMediasCompany.ToListAsync();
        }
    }
}
