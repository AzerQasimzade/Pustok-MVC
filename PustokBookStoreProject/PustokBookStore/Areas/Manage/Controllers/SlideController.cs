﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.Areas.ViewModels;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.Utilities.Enums;
using PustokBookStore.Utilities.Extensions;

namespace PustokBookStore.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> Sliders = await _context.Sliders.ToListAsync();
            return View(Sliders);
        }
        public async Task<IActionResult> Create()
        { 
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!slideVM.Photo.ValidateFileType(FileHelper.Image))
            {
                ModelState.AddModelError("Photo", "File tipi uygun deyil");
                return View();
            }
            if (!slideVM.Photo.ValidateSize(SizeHelper.mb))
            {
                ModelState.AddModelError("Photo", "File olcusu 1 mb den boyuk olmamalidir");
                return View();
            }
            string filename = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "image", "bg-images");
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
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Slider existed = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            existed.Image.DeleteFile(_env.WebRootPath, "assets", "image", "bg-images");
            _context.Sliders.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
    //public async Task<IActionResult> Update(int id)
    //{
    //    if (id <= 0)
    //    {
    //        return BadRequest();
    //    }
    //    Slider existed = await _context.Slides.FirstOrDefaultAsync(x => x.Id == id);
    //    if (existed is null)
    //    {
    //        return NotFound();
    //    }
    //    UpdateSlideVM slideVM = new UpdateSlideVM
    //    {
    //        Description = existed.Description,
    //        Order = existed.Order,
    //        SubTitle = existed.SubTitle,
    //        Title = existed.Title,
    //        Image = existed.Image
    //    };
    //    return View(slideVM);
    //}
    //[HttpPost]
    //public async Task<IActionResult> Update(int id, UpdateSlideVM slideVM)
    //{
    //    Slider existed = await _context.Slides.FirstOrDefaultAsync(x => x.Id == id);
    //    if (existed is null)
    //    {
    //        return NotFound();
    //    }
    //    if (!ModelState.IsValid)
    //    {
    //        return View(slideVM);
    //    }
    //    if (slideVM.Photo is not null)
    //    {
    //        if (!slideVM.Photo.ValidateFileType(FileHelper.Image))
    //        {
    //            ModelState.AddModelError("Photo", "File tipi uygun deyil");
    //            return View(slideVM);
    //        }
    //        if (!slideVM.Photo.ValidateSize(SizeHelper.mb))
    //        {
    //            ModelState.AddModelError("Photo", "File olcusu 1 mb den boyuk olmamalidir");
    //            return View(slideVM);
    //        }
    //        string filename = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "slider");
    //        existed.Image.DeleteFile(_env.WebRootPath, "assets", "images", "slider");
    //        existed.Image = filename;
    //    }
    //    existed.Title = slideVM.Title;
    //    existed.Description = slideVM.Description;
    //    existed.Order = slideVM.Order;
    //    existed.SubTitle = slideVM.SubTitle;
    //    await _context.SaveChangesAsync();
    //    return RedirectToAction(nameof(Index));
    //}

    //public async Task<IActionResult> Details(int id)
    //{
    //    Slider slide = await _context.Slides
    //        .FirstOrDefaultAsync(x => x.Id == id);
    //    if (slide is null)
    //    {
    //        return NotFound();
    //    }
    //    return View(slide);
    //}

}

