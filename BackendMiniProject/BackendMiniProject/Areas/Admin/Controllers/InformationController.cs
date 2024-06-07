using BackendMiniProject.Data;
using BackendMiniProject.Helpers;
using BackendMiniProject.Models;
using BackendMiniProject.Services;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels.Informations;
using BackendMiniProject.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InformationController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IInformationService _informationService;
        public InformationController(AppDbContext context,
                                IWebHostEnvironment env,
                                IInformationService ınformationService)
        {
            _context = context;
            _env = env;
            _informationService = ınformationService;
        }
        public async Task<IActionResult> Index()
        {
            var slider = await _context.Informations.ToListAsync();
            List<InformationVM> information = await _context.Informations.Select(m => new InformationVM { Id = m.Id, Icon = m.Icon, Title = m.Title, Description = m.Description })
                                                           .ToListAsync();
            return View(information);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InformationCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            if (!request.Icon.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input can accept only image format");
                return View();
            }
            if (!request.Icon.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB");
                return View();
            }



            string fileName = Guid.NewGuid().ToString() + "-" + request.Icon.FileName;

            string path = Path.Combine(_env.WebRootPath, "admin/assets/images", fileName);

            await request.Icon.SaveFileToLocalAsync(path);

            await _context.Informations.AddAsync(new Models.Information { Icon = fileName, Title = request.Title, Description = request.Description });

            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var deleteInform = await _context.Informations.FindAsync(id);
            if (deleteInform == null) return NotFound();

            string path = _env.GenerateFilePath("img", deleteInform.Icon);

            path.DeleteFileFromLocal();

            _context.Informations.Remove(deleteInform);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            Information inform = await _informationService.GetByIdAsync((int)id);


            return View(inform);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var inform = await _context.Informations.FindAsync(id);
            if (inform == null)
            {
                return NotFound();
            }

            var viewModel = new InformationEditVM
            {
                Icon = inform.Icon,
                Title = inform.Title,
                Description = inform.Description

            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, InformationEditVM request)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var information = await _context.Informations.FindAsync(id);
            if (information == null)
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

                string oldPath = _env.GenerateFilePath("admin/assets/images", information.Icon);
                oldPath.DeleteFileFromLocal();
                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newPath = _env.GenerateFilePath("admin/assets/images", fileName);
                await request.NewImage.SaveFileToLocalAsync(newPath);
                information.Icon = fileName;
            }


            information.Title = request.Title;
            information.Description = request.Description;


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
