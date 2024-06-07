
using BackendMiniProject.Models;
using BackendMiniProject.Services;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels;
using BackendMiniProject.ViewModels.Informations;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BackendMiniProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInformationService _informationService;

        public HomeController(IInformationService informationService)
        {
            _informationService = informationService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _informationService.GetAllAsync();
            HomeVM model = new()
            {
                Informations= result.Select(m => new InformationVM
                {
                    Icon = m.Icon,
                    Title = m.Title,
                    Description = m.Description,

                })
                
            };
          

            return View(model);
        }

    }
}
