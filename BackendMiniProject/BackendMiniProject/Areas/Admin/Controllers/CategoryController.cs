using BackendMiniProject.Data;
using BackendMiniProject.Helpers.Extensions;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels.Categories;
using BackendMiniProject.ViewModels.Informations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public CategoryController(ICategoryService categoryService,
                                IWebHostEnvironment env,
                                AppDbContext context)
        {
            _categoryService = categoryService;
            _env = env;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            var mappDatas = _categoryService.GetMappedDatas(categories);
            return View(mappDatas);
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existCategory = await _categoryService.ExistAsync(category.Name);
            if (existCategory)
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View();
            }


            if (!category.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input can accept only image format");
                return View();
            }
            if (!category.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB ");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + category.Image.FileName;

            string path = Path.Combine(_env.WebRootPath, "admin/assets/images/", fileName);
            await category.Image.SaveFileToLocalAsync(path);


            await _categoryService.CreateAsync(new Category { Name = category.Name, Image = fileName });
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var deleteCategory = await _context.Categories.FindAsync(id);
            if (deleteCategory == null) return NotFound();

            string path = _env.GenerateFilePath("admin/assets/images", deleteCategory.Image);

            path.DeleteFileFromLocal();

            _context.Categories.Remove(deleteCategory);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }
    }

}

