﻿
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
        private readonly ICategoryService _categoryService;
        private readonly ICourseService _courseService;

        public HomeController(IInformationService informationService, 
                              IAboutService aboutService,
                              ICategoryService categoryService,
                              ICourseService courseService)
        {
            _informationService = informationService;
            _aboutService = aboutService;
            _categoryService = categoryService;
            _courseService = courseService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _informationService.GetAllAsync();
            var res = await _aboutService.GetAboutAsync();
            var m = await _categoryService.GetAllAsync();
            HomeVM model = new()
            {
                Informations = result.Select(m => new InformationVM
                {
                    Icon = m.Icon,
                    Title = m.Title,
                    Description = m.Description,

                }),


                Abouts= res,
                CategoryFirst = m.FirstOrDefault(),
                CategoryLast = m.LastOrDefault(),
                Categories = m.Skip(1).Take(2),
                Courses = await _courseService.GetAllAsync(),

            };
   
            return View(model);

        }


    }

}


