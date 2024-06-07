using BackendMiniProject.Data;
using BackendMiniProject.Helpers;
using BackendMiniProject.Models;
using BackendMiniProject.Services;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISliderService _sliderService;
        public SliderController(AppDbContext context,
                                IWebHostEnvironment env,
                                ISliderService sliderService)
        {
            _context = context;
            _env = env;
            _sliderService = sliderService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var slider = await _context.Sliders.ToListAsync();
            List<SliderVM> sliders = await _context.Sliders.Select(m => new SliderVM { Id = m.Id, Image = m.Image,Title = m.Title ,Description =m.Description })
                                                           .ToListAsync();
            return View(sliders);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
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

                string path = Path.Combine(_env.WebRootPath, "admin/assets/images", fileName);

                await request.Image.SaveFileToLocalAsync(path);

                await _context.Sliders.AddAsync(new Models.Slider { Image = fileName,Title=request.Title,Description=request.Description});

                await _context.SaveChangesAsync();
            


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var deletedSlider = await _context.Sliders.FindAsync(id);
            if (deletedSlider == null) return NotFound();

            string path = _env.GenerateFilePath("img", deletedSlider.Image);

            path.DeleteFileFromLocal();

            _context.Sliders.Remove(deletedSlider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            Slider slider = await _sliderService.GetByIdAsync((int)id);


            return View(slider);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            var viewModel = new SliderEditVM
            {
                Image = slider.Image,
                Title = slider.Title,
                Description = slider.Description
             
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderEditVM request)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
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

                string oldPath = _env.GenerateFilePath("admin/assets/images", slider.Image);
                oldPath.DeleteFileFromLocal();
                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newPath = _env.GenerateFilePath("admin/assets/images", fileName);
                await request.NewImage.SaveFileToLocalAsync(newPath);
                slider.Image = fileName;
            }


            slider.Title = request.Title;
            slider.Description = request.Description;
         

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }


}
