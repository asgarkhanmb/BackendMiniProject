using System.ComponentModel.DataAnnotations;

namespace BackendMiniProject.ViewModels.Courses
{
    public class CourseCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string Price { get; set; }
        public int InstructorId { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
