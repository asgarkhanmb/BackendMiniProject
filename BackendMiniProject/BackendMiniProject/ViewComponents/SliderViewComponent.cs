using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendMiniProject.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ISliderService _sliderService;

        public SliderViewComponent(ISliderService sliderService)
        {

            _sliderService = sliderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliderDatas = new SliderVMVC
            {
                Sliders = await _sliderService.GetAllAsync(),
            };

            return await Task.FromResult(View(sliderDatas));
        }

    }
    public class SliderVMVC
    {
        public IEnumerable<Slider> Sliders { get; set; }
       
    }
}

