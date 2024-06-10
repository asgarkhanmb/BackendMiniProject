using BackendMiniProject.Data;
using BackendMiniProject.Helpers.Extensions;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackendMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CourseController : Controller
    {

        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IInstructorService _instructorService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public CourseController(ICourseService courseService,
                                IWebHostEnvironment env,
                                ICategoryService categoryService,
                                IInstructorService instructorService,
                                AppDbContext context)
        {
            _courseService = courseService;
            _env = env;
            _categoryService = categoryService;
            _instructorService = instructorService;
            _context = context;
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
            ViewBag.instructor = await _instructorService.GetAllSelectedAsync();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var deleteInform = await _context.Courses.FindAsync(id);
            if (deleteInform == null) return NotFound();

            string path = _env.GenerateFilePath("admin/assets/images", deleteInform.Name);
            path.DeleteFileFromLocal();

            _context.Courses.Remove(deleteInform);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id is null) return BadRequest();

            var existProduct = await _courseService.GetByIdWithCoursesImagesAsync((int)id);

            if (existProduct is null) return NotFound();



            ViewBag.categories = await _categoryService.GetAllSelectedAsync();

            List<CourseImageVM> images = new();

            foreach (var item in existProduct.CoursesImages)
            {
                images.Add(new CourseImageVM
                {
                    Id = item.Id,
                    Image = item.Name,
                    IsMain = item.IsMain
                });
            }

            CourseEditVM response = new()
            {
                Name = existProduct.Name,
                Duration = existProduct.Duration,
                Rating = existProduct.Rating,
                Images = images,
                CategoryId = existProduct.CategoryId,
            };





            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CourseEditVM request)
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            if (!ModelState.IsValid)
            {
                var product = await _courseService.GetByIdAsync((int)id);

                List<CourseImageVM> images = new();

                foreach (var item in product.CoursesImages)
                {
                    images.Add(new CourseImageVM
                    {
                        Image = item.Name,
                        IsMain = item.IsMain
                    });
                }

                return View(new CourseEditVM { Images = images });

            }

            if (id == null) return BadRequest();
            var products = await _courseService.GetByIdWithCoursesImagesAsync((int)id);
            if (products == null) return NotFound();


            if (request.NewImages is not null)
            {

                List<CourseImage> images = new();
                foreach (var item in request.NewImages)
                {
                    string fileName = $"{Guid.NewGuid()}-{item.FileName}";
                    string path = _env.GenerateFilePath("admin/assets/images", fileName);
                    await item.SaveFileToLocalAsync(path);
                    images.Add(new CourseImage { Name = fileName });
                }

                foreach (var item in request.NewImages)
                {


                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("NewImages", "Input can accept only image format");
                        products.CoursesImages = images;
                        return View(request);

                    }
                    if (!item.CheckFileSize(500))
                    {
                        ModelState.AddModelError("NewImages", "Image size must be max 500 KB ");
                        products.CoursesImages = images;
                        return View(request);
                    }


                }
                foreach (var item in request.NewImages)
                {
                    string oldPath = _env.GenerateFilePath("admin/assets/images", item.Name);
                    oldPath.DeleteFileFromLocal();
                    string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                    string newPath = _env.GenerateFilePath("admin/assets/images", fileName);

                    await item.SaveFileToLocalAsync(newPath);

                    products.CoursesImages.Add(new CourseImage { Name = fileName });

                }

            }

            if (request.Name is not null)
            {
                products.Name = request.Name;
            }
            if (request.Duration is not null)
            {
                products.Duration = (int)request.Duration;
            }
            if (request.Rating is not null)
            {
                products.Duration = (int)request.Rating;
            }
            if (request.CategoryId != 0)
            {
                products.CategoryId = request.CategoryId;
            }
            if (request.Price is not null)
            {
                products.Price = decimal.Parse(request.Price);
            }

            await _courseService.UpdatedAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            var existProduct = await _courseService.GetByIdWithCoursesImagesAsync((int)id);
            if (existProduct is null) return NotFound();
            var category = await _categoryService.GetByIdAsync(existProduct.CategoryId);

            List<CourseImageVM> images = new();
            foreach (var item in existProduct.CoursesImages)
            {
                images.Add(new CourseImageVM
                {
                    Image = item.Name,
                    IsMain = item.IsMain

                });
            }
            CourseDetailVM response = new()
            {
                Name = existProduct.Name,
                Rating = existProduct.Rating,
                Category = category.Name,
                Price = existProduct.Price,
                Duration = existProduct.Duration,
                Images = images,
            };
            return View(response);
        }

    }
}
