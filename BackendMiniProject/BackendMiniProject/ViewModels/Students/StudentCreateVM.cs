namespace BackendMiniProject.ViewModels.Students
{
    public class StudentCreateVM
    {
        public string FullName { get; set; }
        public string? Biography { get; set; }
        public string Profession { get; set; }
        public IFormFile Image { get; set; }
    }
}
