using BackendMiniProject.Models;
using BackendMiniProject.ViewModels.Informations;
using BackendMiniProject.ViewModels.Sliders;

namespace BackendMiniProject.ViewModels
{

    public class HomeVM
    {
        public IEnumerable<SliderVM> Sliders {  get; set; }
        public IEnumerable<InformationVM> Informations { get; set; }

        public About Abouts { get; set; }

        public Category CategoryFirst { get; set; }
        public Category CategoryLast { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Instructor> Instructors { get; set; }
        
    }
}
