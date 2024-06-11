
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
        private readonly IInstructorService _instructorService;
        private readonly IStudentService _studentService;

        public HomeController(IInformationService informationService,
                              IAboutService aboutService,
                              ICategoryService categoryService,
                              ICourseService courseService,
                              IInstructorService instructorService,
                              IStudentService studentService)
        {
            _informationService = informationService;
            _aboutService = aboutService;
            _categoryService = categoryService;
            _courseService = courseService;
            _instructorService = instructorService;
            _studentService = studentService;
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


                Abouts = res,
                CategoryFirst = m.FirstOrDefault(),
                CategoryLast = m.LastOrDefault(),
                Categories = m.Skip(1).Take(2),
                Courses = await _courseService.GetAllAsync(),
                Instructors = await _instructorService.GetAllAsync(),
                Students = await _studentService.GetAllAsync(),

            };

            return View(model);

        }


    }

}


