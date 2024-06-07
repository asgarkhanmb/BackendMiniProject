using System.ComponentModel.DataAnnotations;

namespace BackendMiniProject.ViewModels.Sliders
{

    public class SliderCreateVM
    {
        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

    }

}
