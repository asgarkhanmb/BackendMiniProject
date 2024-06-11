using BackendMiniProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseImage> CourseImages { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<InstructorSocial> InstructorSocials { get; set; }

        public DbSet<Social> Socials { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SocialMediaCompany> SocialMediasCompany { get; set; }


    }
}
