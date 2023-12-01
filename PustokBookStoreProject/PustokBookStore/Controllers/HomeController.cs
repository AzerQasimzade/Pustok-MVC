using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.ViewModels;

namespace PustokBookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> Sliders = await _context.Sliders.OrderBy(x => x.Order).ToListAsync();
            List<Feature> Features =await _context.Features.ToListAsync();
            List<Book> books =await _context.Books
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.BookImages)  
                .ToListAsync();
            HomeVM homeVM = new HomeVM
            {
                Sliders = Sliders,
                Features = Features,
                Books = books,
                DiscountBooks=books.Where(x=>x.Discount>0).Take(5).ToList(),
                NewBooks=books.OrderByDescending(x=>x.Id).Take(5).ToList(),
                ExpenciveBooks=books.OrderByDescending(x=>x.SalePrice).Take(5).ToList(),

            };
            return View(homeVM);
        }
    }
    }

       

        
    

