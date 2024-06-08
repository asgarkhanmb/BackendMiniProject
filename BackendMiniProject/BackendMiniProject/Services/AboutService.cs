using BackendMiniProject.Data;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Services
{
    public class AboutService : IAboutService
    {
        private readonly AppDbContext _context;
        public AboutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<About> GetAboutAsync()
        {
            return await _context.Abouts.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<About>> GetAllAsync()
        {
           return await _context.Abouts.ToListAsync();
        }

        public async Task<About> GetByIdAsync(int id)
        {
           return await _context.Abouts.Where(m => m.Id == id).FirstOrDefaultAsync();
        }
    }
}
