using BackendMiniProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Slider> Sliders { get; set; }

    }
}
