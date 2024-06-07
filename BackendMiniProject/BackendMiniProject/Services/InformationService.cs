using BackendMiniProject.Data;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Services
{
    public class InformationService : IInformationService
    {
        private readonly AppDbContext _context;
        public InformationService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Information>> GetAllAsync()
        {
            return await _context.Informations.ToListAsync();
        }

        public async Task<Information> GetByIdAsync(int id)
        {
            return await _context.Informations.Where(m => m.Id == id).FirstOrDefaultAsync();
        }
    }
}
