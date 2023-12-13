using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.Areas.ViewModels;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.Utilities.Extensions;
using static System.Reflection.Metadata.BlobBuilder;

namespace PustokBookStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]

    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int count = await _context.Sliders.CountAsync();
            List<Slider> Sliders = await _context.Sliders
                .Skip((page - 1) * 3).Take(3).ToListAsync();
            PaginationVM<Slider> paginationVM = new PaginationVM<Slider>()
            {
                Items = Sliders,
                CurrentPage = page,
                TotalPage = Math.Ceiling((double)count / 3)
            };

            return View(paginationVM);
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

            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Photo can be must image type");
                return View();
            }

            if (slider.Photo.CheckFileLength(300))
            {
                ModelState.AddModelError("Photo", "Photo can not be than " + 300 + " kb");
                return View();
            }




            slider.Image = slider.Photo.CreateFile(_env.WebRootPath, "assets/image/bg-images");

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

            if (slider is null) return NotFound();

            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Slider slider)
        {
            Slider exist = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

            if (exist is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(exist);
            }

            if (slider.Photo is not null)
            {

                if (!slider.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Photo can be must image type");
                    return View();
                }

                if (slider.Photo.CheckFileLength(300))
                {
                    ModelState.AddModelError("Photo", "Photo can not be than " + 300 + " kb");
                    return View();
                }

                exist.Image.DeleteFile(_env.WebRootPath, "uploads/slider");

                exist.Image = slider.Photo.CreateFile(_env.WebRootPath, "uploads/slider");
            }

            exist.Title1 = slider.Title1;
            exist.Title2 = slider.Title2;
            exist.Desc = slider.Desc;
            exist.Order = slider.Order;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

            if (slider is null) return NotFound();

            slider.Image.DeleteFile(_env.WebRootPath, "uploads/slider");



            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}

