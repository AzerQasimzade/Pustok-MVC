using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.Areas.ViewModels;
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

        public async Task<IActionResult> Create()
        { 
            Slider slider = _context.Sliders.FirstOrDefault();
      
            CreateSlideVM slideVM = new CreateSlideVM
            {
                Title1 = slider.Title1,
                Title2 = slider.Title2,
                Desc = slider.Desc,
                Order = slider.Order,
                Photo = slider.Photo,       
            };
            return View(slideVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View(slideVM);
            }

            if (!slideVM.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Photo can be must image type");
                return View(slideVM);
            }

            if (slideVM.Photo.Length > 200 * 1024)
            {
                ModelState.AddModelError("Photo", "Photo can not be than 200 kb");
                return View(slideVM);
            }

            FileStream stream = new FileStream(@"C:\Users\ca.r214.03\Desktop\PustokBookStoreMain\wwwroot\assets\images\bg-images\" + slideVM.Photo.FileName, FileMode.Create);
            slideVM.Photo.CopyTo(stream);
            Slider slider = new Slider
            {
                Title1 = slideVM.Title1,
                Title2=slideVM.Title2,
                Desc = slideVM.Desc,
                Order = slideVM.Order,
                Photo = slideVM.Photo,
            };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
