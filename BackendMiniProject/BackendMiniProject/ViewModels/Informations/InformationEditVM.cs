namespace BackendMiniProject.ViewModels.Informations
{
    public class InformationEditVM
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
