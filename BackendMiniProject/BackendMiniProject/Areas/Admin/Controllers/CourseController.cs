using BackendMiniProject.Helpers.Extensions;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackendMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {

        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IInstructorService _instructorService;
        private readonly IWebHostEnvironment _env;
        public CourseController(ICourseService courseService,
                                IWebHostEnvironment env,
                                ICategoryService categoryService,
                                IInstructorService instructorService)
        {
            _courseService = courseService;
            _env = env;
            _categoryService = categoryService;
            _instructorService = instructorService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();
            var mappDatas = _courseService.GetMappedDatas(courses);
            return View(mappDatas);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            ViewBag.instructor =new SelectList(await _instructorService.GetAllSelectedAsync(),"Id","Name");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM request)
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            ViewBag.instructor = await _instructorService.GetAllSelectedAsync();
            if (!ModelState.IsValid)
            {
                return View();

            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Images", "Image size must be max 500 KB");
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be only image");

                    return View();
                }
            }
            List<CourseImage> images = new();
            foreach (var item in request.Images)
            {
                string fileName = $"{Guid.NewGuid()}-{item.FileName}";
                string path = _env.GenerateFilePath("admin/assets/images", fileName);
                await item.SaveFileToLocalAsync(path);
                images.Add(new CourseImage { Name = fileName });
            }

            images.FirstOrDefault().IsMain = true;
            Course category = new()
            {
                Name = request.Name,
                Duration = request.Duration,
                Rating = request.Rating,
                InstructorId = request.InstructorId,
                CategoryId = request.CategoryId,
                Price = decimal.Parse(request.Price.Replace(".", ",")),
                CoursesImages = images

            };

            await _courseService.CreateAsync(category);


            return RedirectToAction(nameof(Index));
        }

    }
}
