using BackendMiniProject.Data;
using BackendMiniProject.Helpers.Extensions;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SocialController : Controller
    {


        private readonly ISocialService _socialService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public SocialController(ISocialService socialService,
                                IWebHostEnvironment env,
                                AppDbContext context)
        {
            _socialService = socialService;
            _env = env;
            _context = context;
        }
        public async  Task<IActionResult> Index()
        {
            var social = await _socialService.GetAllAsync();
            return View(social);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Social request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }




            bool existSocial = await _socialService.ExistAsync(request.Name);
            if (existSocial)
            {
                ModelState.AddModelError("Title", "This title already exist");
                return View();
            }
            await _socialService.CreateAsync(new Social { Name = request.Name });
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var deleteSocial = await _context.Socials.FindAsync(id);
            if (deleteSocial == null) return NotFound();

            string path = _env.GenerateFilePath("admin/assets/images", deleteSocial.Name);

            path.DeleteFileFromLocal();

            _context.Socials.Remove(deleteSocial);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id, Social request)
        {
            if (id == null) return BadRequest();
            var social = await _socialService.GetByIdAsync((int)id);
            if (social == null) return NotFound();


            if (!ModelState.IsValid)
            {
                return View();
            }


            if (request.Name is not null)
            {
                social.Name = request.Name;
            }


            await _socialService.EditAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
