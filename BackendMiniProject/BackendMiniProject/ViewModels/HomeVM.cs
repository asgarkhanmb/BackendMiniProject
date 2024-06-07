using BackendMiniProject.ViewModels.Informations;
using BackendMiniProject.ViewModels.Sliders;

namespace BackendMiniProject.ViewModels
{

    public class HomeVM
    {
        public IEnumerable<SliderVM> Sliders {  get; set; }
        public IEnumerable<InformationVM> Informations { get; set; }
    }
}
