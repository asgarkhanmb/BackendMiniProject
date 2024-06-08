using BackendMiniProject.Data;
using BackendMiniProject.Helpers.Extensions;
using BackendMiniProject.Models;
using BackendMiniProject.Services;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels.Abouts;
using BackendMiniProject.ViewModels.Informations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IAboutService _aboutService;
        public AboutController(AppDbContext context,
                                IWebHostEnvironment env,
                                IAboutService aboutService)
        {
            _context = context;
            _env = env;
            _aboutService = aboutService;
            
        }
        public async  Task<IActionResult> Index()
        {
            var about = await _context.Abouts.ToListAsync();
            List<AboutVM> aboutt = await _context.Abouts.Select(m => new AboutVM { Id = m.Id, Image = m.Image, Title = m.Title, Description = m.Description })
                                                        .ToListAsync();
            return View(aboutt);
         
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input can accept only image format");
                return View();
            }
            if (!request.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB");
                return View();
            }



            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;

            string path = Path.Combine(_env.WebRootPath, "admin/assets/images/", fileName);

            await request.Image.SaveFileToLocalAsync(path);

            await _context.Abouts.AddAsync(new Models.About { Image = fileName, Title = request.Title, Description = request.Description });

            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var deleteAbout = await _context.Abouts.FindAsync(id);
            if (deleteAbout == null) return NotFound();

            string path = _env.GenerateFilePath("admin/assets/images/", deleteAbout.Image);

            path.DeleteFileFromLocal();

            _context.Abouts.Remove(deleteAbout);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            About about = await _aboutService.GetByIdAsync((int)id);


            return View(about);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var result = await _context.Abouts.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            var viewModel = new AboutEditVM
            {
                Image = result.Image,
                Title = result.Title,
                Description = result.Description

            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AboutEditVM request)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var about = await _context.Abouts.FindAsync(id);
            if (about == null)
            {
                return NotFound();
            }

            if (request.NewImage != null)
            {

                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Accept only image format");
                    return View(request);
                }
                if (!request.NewImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 200 KB");
                    return View(request);
                }

                string oldPath = _env.GenerateFilePath("admin/assets/images/", about.Image);
                oldPath.DeleteFileFromLocal();
                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newPath = _env.GenerateFilePath("admin/assets/images/", fileName);
                await request.NewImage.SaveFileToLocalAsync(newPath);
                about.Image = fileName;
            }


            about.Title = request.Title;
            about.Description = request.Description;


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
