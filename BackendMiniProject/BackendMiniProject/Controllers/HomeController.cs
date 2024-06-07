
using BackendMiniProject.Models;
using BackendMiniProject.Services;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BackendMiniProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;

        public HomeController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM model = new()
            {
                
            };
          

            return View(model);
        }

    }
}
