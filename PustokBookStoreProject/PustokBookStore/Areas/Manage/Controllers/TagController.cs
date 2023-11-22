using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.DAL;
using PustokBookStore.Models;

namespace PustokBookStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Tags> tag = await _context.Tags
                .Include(x=>x.Booktags)
                .ToListAsync();
            return View(tag);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Tags tag)
        {
            bool result = await _context.Tags.AnyAsync(x => x.Name == tag.Name);

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (result)
            {
                ModelState.AddModelError("Fullname", "Eyni adli yazici yarana bilmez");
                return View();
            }
            await _context.AddAsync(tag);
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
        public async Task<IActionResult> Delete(int id)
        {
            Tags tag =await _context.Tags.FirstOrDefaultasync(x => x.Id == id);
        }

    }
}
