using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.DAL;
using PustokBookStore.Models;

namespace PustokBookStore.ViewComponents
{
    public class BookViewComponent:ViewComponent
    {   
            private readonly AppDbContext _context;

            public BookViewComponent(AppDbContext context)
            {
                _context = context;
            }
            public async Task<IViewComponentResult>InvokeAsync(string tagname)
            {
                List<Book> book= await _context.Books
               
                .Include(x=>x.Booktags)
                .Where(x=>x.Booktags.Any(y=>y.Tag.Name==tagname))
                .ToListAsync();

            return View(book);
            }
        }

}

