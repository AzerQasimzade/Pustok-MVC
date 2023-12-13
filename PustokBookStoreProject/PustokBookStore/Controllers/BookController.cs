using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.Utilities.Exceptions;
using PustokBookStore.ViewModels;

namespace PustokBookStore.Controllers
{
    public class BookController : Controller
    {

        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                throw new WrongRequestHandlerException("Bad Reguest Sended!");
            }

            Book book = await _context.Books
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.BookImages)
                .Include(x=>x.Booktags)
                .ThenInclude(x=>x.Tag)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (book is null)
            {
                throw new NotFoundException("Page CanNot Found!");
            }
            List<Book> relatedbooks= await _context.Books.Where(x=>x.GenreId==book.GenreId && x.Id!=book.Id)
                .Include(x=>x.Genre)
                .Include(x=>x.BookImages)  
                .Include(x=>x.Author)
                .ToListAsync();
            DetailVM detailVM = new DetailVM
            {
                Book = book,
                RelatedBooks = relatedbooks,
            };
            return View(detailVM);
        }
    }
}
