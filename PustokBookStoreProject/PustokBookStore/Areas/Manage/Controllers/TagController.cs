﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.Areas.ViewModels;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace PustokBookStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    //[Authorize(Roles ="Admin")]

    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            int count =await _context.Tags.CountAsync();
            List<Tags> tag = await _context.Tags.Skip((page - 1) * 3).Take(3)
                .Include(x=>x.Booktags)
                .ToListAsync();
            PaginationVM<Tags> paginationVM = new PaginationVM<Tags>()
            {
                Items = tag,
                CurrentPage = page,
                TotalPage = Math.Ceiling((double)count / 3)
            };
            return View(paginationVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVM tagVM)
        {
            bool result = await _context.Tags.AnyAsync(x => x.Name == tagVM.Name);

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (result)
            {
                ModelState.AddModelError("Fullname", "Eyni adli yazici yarana bilmez");
                return View();
            }
            await _context.AddAsync(tagVM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Tags tag =_context.Tags.FirstOrDefault(x => x.Id == id);
            if (tag == null)
            {
               return NotFound();
            }
          return View(tag);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Tags tag1)
        {
            Tags tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (tag == null)
            {
                return NotFound();
            }
            bool result= await _context.Tags.AnyAsync(y => y.Name == tag1.Name && y.Id==id);
            if (result) 
            {
                ModelState.AddModelError("Name", "Eyni adli yazici yarana bilmez");
                return View();
            }
            tag.Name=tag1.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }     
    }
}
