namespace BackendMiniProject.ViewModels.Courses
{
    public class CourseDetailVM
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public List<CourseImageVM> Images { get; set; }
    }
}
