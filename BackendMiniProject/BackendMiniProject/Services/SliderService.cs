using BackendMiniProject.Data;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        public SliderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Slider>> GetAllAsync()
        {
            return await _context.Sliders.ToListAsync();
        }

        public async Task<Slider> GetByIdAsync(int id)
        {
            return await _context.Sliders.Where(m => m.Id == id).FirstOrDefaultAsync();
        }
    }
}
