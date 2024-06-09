namespace BackendMiniProject.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
        public string Image {  get; set; }
    }
}
