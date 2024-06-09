namespace BackendMiniProject.Models
{
    public class Student : BaseEntity
    {
        public string FullName { get; set; }
        public string? Biography { get; set; }
        public string Profession { get; set; }
        public string Image { get; set; }
    }
}
