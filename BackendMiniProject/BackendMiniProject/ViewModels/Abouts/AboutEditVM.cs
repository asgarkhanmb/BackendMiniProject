namespace BackendMiniProject.ViewModels.Abouts
{
    public class AboutEditVM
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
