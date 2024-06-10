namespace BackendMiniProject.ViewModels.Instructors
{
    public class InstructorEditVM
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Designation { get; set; }
        public string? Images { get; set; }

        public IFormFile? NewImages { get; set; }
    }
}
