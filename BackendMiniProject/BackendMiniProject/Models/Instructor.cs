namespace BackendMiniProject.Models
{
    public class Instructor : BaseEntity
    {
        public string FullName { get; set; }
        public string Image {  get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public List<InstructorSocial> InstructorSocials { get; set; }

    }
}
