using Microsoft.AspNetCore.Authorization;
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
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            int count = await _context.Books.CountAsync();
            List<Author> authors = await _context.Author.Skip((page - 1) * 3).Take(3)
                .Include(x => x.Books).ToListAsync();
            PaginationVM<Author> paginationVM = new PaginationVM<Author>()
            {
                Items = authors,
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
        public async Task<IActionResult> Create(Author author)
        {
            bool result = await _context.Author.AnyAsync(x => x.FullName == author.FullName);

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (result)
            {
                ModelState.AddModelError("Fullname", "Eyni adli yazici yarana bilmez");
                return View();
            }

            await _context.AddAsync(author);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Author author = await _context.Author.FirstOrDefaultAsync(x => x.Id == id);

            if (author == null) return NotFound();

            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Author author)
        {
            Author exist = await _context.Author.FirstOrDefaultAsync(x => x.Id == id);

            if (exist == null) return NotFound();

            bool result = await _context.Author.AnyAsync(x => x.FullName == author.FullName);

            if (!ModelState.IsValid)
            {
                return View(exist);
            }

            if (result)
            {
                ModelState.AddModelError("Fullname", "Eyni adli yazici yarana bilmez");
                return View(exist);
            }
            exist.FullName = author.FullName;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }                                                                                                                                        
        public async Task<IActionResult> Delete(int id)
        {
            Author exist = await _context.Author.FirstOrDefaultAsync(x => x.Id == id);

            if (exist == null) return NotFound();

            _context.Author.Remove(exist);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}