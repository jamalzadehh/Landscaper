using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.DAL;
using Practice.Models;
using Practice.ViewModels;
using Practice.ViewModels.Service;
using System.IO;

namespace Practice.Areas.admin.Controllers
{
    [Area("admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var service = await _context.Services.ToListAsync();
            return View(service);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ServiceCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var isExisted = await _context.Services.AnyAsync(x => x.FullName.ToLower().Contains(vm.FullName.ToLower()));
            if (!isExisted)
            {
                ModelState.AddModelError("FullName", "Bu servis movcuddur");
            }
            if (!vm.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "File tipi yanlisdir");
            }
            if (vm.Image.Length < 4 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Seklin olcusu 4 mb dan az olmalidir");
            }
            string filename = Guid.NewGuid() + vm.Image.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets", "img", filename);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await vm.Image.CopyToAsync(stream);
            }
            Service service = new()
            {
                FullName = vm.FullName,
                Description = vm.Description,
                ImageUrl = filename
            };

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (existed == null) return NotFound();
            ServiceUpdateVM vm = new()
            {
                Id = id,
                FullName = existed.FullName,
                Description = existed.Description,
                ImageUrl = existed.ImageUrl,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ServiceUpdateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var existed = await _context.Services.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (existed == null) return NotFound();
            var isExisted = await _context.Services.AnyAsync(x => x.FullName.ToLower().Contains(vm.FullName.ToLower()) && x.Id != vm.Id);
            if (isExisted)
            {
                ModelState.AddModelError("FullName", "Bu servis movcuddur");
            }
            if (vm.Image is not null)
            {
                string filename = Guid.NewGuid() + vm.Image.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "img");
                if (System.IO.File.Exists(path + "/" + existed.ImageUrl))
                {
                    System.IO.File.Delete(path + "/" + existed.ImageUrl);
                }
                using (FileStream stream = new(path + "/" + filename, FileMode.Create))
                {
                    await vm.Image.CopyToAsync(stream);
                }
                filename = existed.ImageUrl;
            }
            existed.FullName = vm.FullName;
            existed.Description = vm.Description;

            _context.Services.Update(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (existed == null) return NotFound();
            string path= Path.Combine(_env.WebRootPath,"assets/img",existed.ImageUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Services.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
