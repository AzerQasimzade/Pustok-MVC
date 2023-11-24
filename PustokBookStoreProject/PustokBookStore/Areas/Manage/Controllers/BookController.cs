using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.Areas.ViewModels;
using PustokBookStore.DAL;
using PustokBookStore.Models;

namespace PustokBookStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Book> books = await _context.Books
                .Include(x=>x.Author)
                .Include(x=>x.BookImages.Where(x=>x.IsPrimary==true))
                .Include(x=>x.Genre)
     
                .ToListAsync();
            return View(books);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Genre = await _context.Genres.ToListAsync();
            ViewBag.Author=await _context.Author.ToListAsync(); 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookCreateVM bookVM)
        {
            ViewBag.Genre = await _context.Genres.ToListAsync();
            ViewBag.Author = await _context.Author.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(bookVM);
            }
            if (!await _context.Author.AnyAsync(x => x.Id == bookVM.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Bu id de shair Movcud deyil.");
                return View();
            }
            if (!await _context.Genres.AnyAsync(x => x.Id == bookVM.GenreId))
            {
                ModelState.AddModelError("GenreId", "Bu id de janr Movcud deyil.");
                return View();
            }
            Book book = new Book
            {
                Name=bookVM.Name,
                Desc=bookVM.Desc,
                CostPrice=bookVM.CostPrice,
                SalePrice=bookVM.SalePrice,
                Discount=bookVM.Discount,
                IsDeleted=bookVM.IsDeleted,
                GenreId=bookVM.GenreId,
                AuthorId=bookVM.AuthorId
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
