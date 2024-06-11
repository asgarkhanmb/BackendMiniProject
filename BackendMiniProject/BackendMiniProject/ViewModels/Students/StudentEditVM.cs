namespace BackendMiniProject.ViewModels.Students
{
    public class StudentEditVM
    {
        public string Image { get; set; }
        public IFormFile? NewImage { get; set; }



        public string Biography { get; set; }
        public string FullName { get; set; }


        public string Profession { get; set; }
    }
}
