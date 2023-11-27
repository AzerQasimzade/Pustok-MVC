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
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Book book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            UpdateBookVM bookVM = new UpdateBookVM
            {
                Name = book.Name,
                Desc = book.Desc,
                CostPrice = book.CostPrice,
                SalePrice = book.SalePrice,
                Discount = book.Discount,
                IsAvailable=book.IsAvailable,
                GenreId=book.GenreId,
                AuthorId=book.AuthorId,
                Page=book.Page,
                Authors = await _context.Author.ToListAsync(),
                Genres= await _context.Genres.ToListAsync(),
            };
            return View(bookVM);  
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateBookVM bookVM)
        {
           
            if (!ModelState.IsValid)
            {
                bookVM.Authors = await _context.Author.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
                ModelState.AddModelError("Book", "We have not so Book with this Id");
                return View(bookVM);
            }
            Book existed = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null)
            {
                return NotFound();          
            }
            bool result = await _context.Author.AnyAsync(x => x.Id == bookVM.AuthorId);
            if (result)
            {
                bookVM.Authors = await _context.Author.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
                ModelState.AddModelError("Author", "Bu id de author movcud deyil");
                return View();
            }
            bool gresult = await _context.Genres.AnyAsync(x => x.Id == bookVM.GenreId);
            if (gresult)
            {
                bookVM.Authors = await _context.Author.ToListAsync();
                bookVM.Genres = await _context.Genres.ToListAsync();
                ModelState.AddModelError("Genre", "Bu id de Genre movcud deyil");
                return View();
            }
            existed.Name = bookVM.Name;
            existed.SalePrice = bookVM.SalePrice;
            existed.CostPrice = bookVM.CostPrice;
            existed.Desc=bookVM.Desc;
            existed.Discount = bookVM.Discount;
            existed.IsAvailable = bookVM.IsAvailable;
            existed.Page = bookVM.Page;  
            existed.AuthorId = bookVM.AuthorId;
            
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}
