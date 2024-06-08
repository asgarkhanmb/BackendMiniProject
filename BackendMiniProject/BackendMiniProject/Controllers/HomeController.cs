
using BackendMiniProject.Models;
using BackendMiniProject.Services;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels;
using BackendMiniProject.ViewModels.Abouts;
using BackendMiniProject.ViewModels.Informations;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BackendMiniProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInformationService _informationService;
        private readonly IAboutService _aboutService;

        public HomeController(IInformationService informationService, 
                              IAboutService aboutService)
        {
            _informationService = informationService;
            _aboutService = aboutService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _informationService.GetAllAsync();
            var res = await _aboutService.GetAboutAsync();
            HomeVM model = new()
            {
                Informations = result.Select(m => new InformationVM
                {
                    Icon = m.Icon,
                    Title = m.Title,
                    Description = m.Description,

                }),


                Abouts= res 
            };
   
            return View(model);

        }


    }

}


