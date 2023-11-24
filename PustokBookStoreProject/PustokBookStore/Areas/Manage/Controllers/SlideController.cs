using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.DAL;
using PustokBookStore.Models;

namespace PustokBookStore.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> Sliders = await _context.Sliders.ToListAsync();
            return View(Sliders);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!slider.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Photo can be must image type");
                return View();
            }

            if (slider.Photo.Length > 200 * 1024)
            {
                ModelState.AddModelError("Photo", "Photo can not be than 200 kb");
                return View();
            }

            FileStream stream = new FileStream(@"C:\Users\lenovo\OneDrive\Masaüstü\AB202\PustokAB202\PustokAB202\wwwroot\uploads\slider\" + slider.Photo.FileName, FileMode.Create);

            slider.Photo.CopyTo(stream);

            slider.Image = slider.Photo.FileName;

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
