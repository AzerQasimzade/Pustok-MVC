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

        public IActionResult Index()
        {
            List<Slider> Sliders = _context.Sliders.OrderBy(x => x.Order).ToList();
            List<Feature> Features = _context.Features.ToList();
            List<Book> Books = _context.Books
                .Include(x=>x.Author)
                .Include(x=>x.Genre)
                .Include(x=>x.BookImages)
                .ToList();
            HomeVM homeVM = new HomeVM
            {
                Sliders = Sliders,
                Features = Features,
                Books = Books
            };
            return View(homeVM);
        }
    }
}
