using BackendMiniProject.Data;
using BackendMiniProject.Helpers.Extensions;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels.Instructors;
using BackendMiniProject.ViewModels.Socials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class InstructorController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IInstructorService _instructorService;
        private readonly IWebHostEnvironment _env;
        private readonly ISocialService _socialService;
        private readonly AppDbContext _context;



        public InstructorController(ICourseService courseService,
                                    ICategoryService categoryService,
                                    IInstructorService instructorService,
                                    IWebHostEnvironment env,
                                    ISocialService socialService,
                                    AppDbContext context)
        {
            _courseService = courseService;
            _categoryService = categoryService;
            _instructorService = instructorService;
            _env = env;
            _socialService = socialService;
            _context = context;


        }
        public async Task<IActionResult> Index()
        {
            var instructor = await _instructorService.GetAllAsync();

            List<InstructorVM> instructors = instructor.Select(m => new InstructorVM { Id = m.Id, FullName = m.FullName, Email = m.Email, Image = m.Image, Designation = m.Designation }).ToList();


            return View(instructors);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(InstructorCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existIns = await _instructorService.ExistEmailAsync(request.Email);

            if (existIns)
            {
                ModelState.AddModelError("Name", "Instructor already exist!");
                return View();
            }
            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input can accept only image format");
                return View();
            }
            if (!request.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB ");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;

            string path = Path.Combine(_env.WebRootPath, "admin/assets/images", fileName);
            await request.Image.SaveFileToLocalAsync(path);
            await _instructorService.CreateAsync(new Instructor { FullName = request.FullName, Image = fileName, Email = request.Email, Designation = request.Designation });
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var deleteInstruc = await _context.Instructors.FindAsync(id);
            if (deleteInstruc == null) return NotFound();

            string path = _env.GenerateFilePath("admin/assets/images", deleteInstruc.Image);

            path.DeleteFileFromLocal();

            _context.Instructors.Remove(deleteInstruc);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            Instructor instructor = await _instructorService.GetByIdWithSocialAsync((int)id);
            return View(instructor);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var instructor = await _instructorService.GetByIdAsync((int)id);
            if (instructor is null) return NotFound();

            return View(new InstructorEditVM { FullName = instructor.FullName, Images = instructor.Image, Email = instructor.Email, Designation = instructor.Designation });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, InstructorEditVM request)
        {
            var a = await _instructorService.GetByIdWithSocialAsync((int)id);
            request.Images = a.Image;

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (request.NewImages is not null)
            {

                if (id is null) return BadRequest();

                if (await _instructorService.ExistExceptByIdAsync((int)id, request.Email))
                {
                    ModelState.AddModelError("Name", "This category already exist");
                    return View();
                }


                var instructor = await _instructorService.GetByIdAsync((int)id);

                if (instructor is null) return NotFound();





                if (request.NewImages is not null)
                {


                    if (!request.NewImages.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("NewImages", "Input can accept only image format");
                        return View(request);

                    }
                    if (!request.NewImages.CheckFileSize(500))
                    {
                        ModelState.AddModelError("NewImages", "Image size must be max 500 KB ");
                        return View(request);
                    }
                    string oldPath = _env.GenerateFilePath("img", request.NewImages.Name);
                    oldPath.DeleteFileFromLocal();
                    string newfileName = Guid.NewGuid().ToString() + "-" + request.NewImages.FileName;
                    string newPath = _env.GenerateFilePath("img", newfileName);

                    await request.NewImages.SaveFileToLocalAsync(newPath);





                    instructor.Image = newfileName;



                }





                if (request.FullName is not null)
                {
                    instructor.FullName = request.FullName;
                }
                if (request.Email is not null)
                {
                    instructor.Email = request.Email;
                }
                if (request.FullName is not null)
                {
                    instructor.Designation = request.Designation;
                }



                await _instructorService.EditAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {


                var instructor = await _instructorService.GetByIdAsync((int)id);

                if (instructor is null) return NotFound();


                if (request.FullName is not null)
                {
                    instructor.FullName = request.FullName;
                }
                if (request.Email is not null)
                {
                    instructor.Email = request.Email;
                }
                if (request.FullName is not null)
                {
                    instructor.Designation = request.Designation;
                }
                await _instructorService.EditAsync();
                return RedirectToAction(nameof(Index));
            }

        }
        [HttpGet]
        public async Task<IActionResult> AddSocial()
        {
            ViewBag.social = await _socialService.GetAllSelectedAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSocial(int? id, SocialCreateVM request)
        {
            ViewBag.social = await _socialService.GetAllSelectedAsync();



            var social = new InstructorSocial { InstructorId = (int)id, SocialId = request.SocialId, SocialLink = request.URl };
            await _context.InstructorSocials.AddAsync(social);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
