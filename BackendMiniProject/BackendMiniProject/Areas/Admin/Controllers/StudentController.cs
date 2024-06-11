using BackendMiniProject.Data;
using BackendMiniProject.Helpers.Extensions;
using BackendMiniProject.Models;
using BackendMiniProject.Services.Interfaces;
using BackendMiniProject.ViewModels.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public StudentController(IStudentService studentService,
                                 IWebHostEnvironment env,
                                 AppDbContext context)
        {
            _studentService = studentService;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _studentService.GetAllAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateVM request)
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
                ModelState.AddModelError("Image", "Image size must be max 200 KB ");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;


            string path = Path.Combine(_env.WebRootPath, "admin/assets/images", fileName);
            await request.Image.SaveFileToLocalAsync(path);



            await _studentService.CreateAsync(new Student { FullName = request.FullName, Profession = request.Profession, Image = fileName, Biography = request.Biography });
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var deleteStudent = await _studentService.GetByIdAsync((int)id);
            if (deleteStudent is null) return NotFound();

            string path = _env.GenerateFilePath("admin/assets/images", deleteStudent.Image);


            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }


            _studentService.DeleteAsync(deleteStudent);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {

            Student student = await _studentService.GetByIdAsync((int)id);
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var student = await _studentService.GetByIdAsync((int)id);
            if (student == null) return NotFound();
            //return View();
            return View(new StudentEditVM { FullName = student.FullName, Profession = student.Profession, Image = student.Image, Biography = student.Biography });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, StudentEditVM request)
        {
            if (id == null) return BadRequest();
            var student = await _studentService.GetByIdAsync((int)id);
            if (student == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (request.NewImage is not null)
            {

                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Input can accept only image format");
                    request.Image = student.Image;
                    return View(request);

                }
                if (!request.NewImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 200 KB ");
                    request.Image = student.Image;
                    return View(request);
                }
                string oldPath = _env.GenerateFilePath("admin/assets/images", student.Image);
                oldPath.DeleteFileFromLocal();
                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newPath = _env.GenerateFilePath("admin/assets/images", fileName);

                await request.NewImage.SaveFileToLocalAsync(newPath);
                if (request.FullName is not null)
                {
                    student.FullName = request.FullName;
                }
                if (request.Profession is not null)
                {
                    student.Profession = request.Profession;
                }
                if (request.Biography is not null)
                {
                    student.Biography = request.Biography;
                }
                if (fileName is not null)
                {
                    student.Image = fileName;
                }
            }
            else
            {
                if (request.FullName is not null)
                {
                    student.FullName = request.FullName;
                }
                if (request.Profession is not null)
                {
                    student.Profession = request.Profession;
                }
                if (request.Biography is not null)
                {
                    student.Biography = request.Biography;
                }
            }



            await _studentService.EditAsync();
            return RedirectToAction(nameof(Index));
        }





    }
}
