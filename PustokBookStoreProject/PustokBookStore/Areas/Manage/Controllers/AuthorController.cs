using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.DAL;
using PustokBookStore.Models;

namespace PustokBookStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Author> authors = await _context.Author.Include(x => x.Books).ToListAsync();
            return View(authors);
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
    }
}