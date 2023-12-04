using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.ViewModels;

namespace PustokBookStore.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddBasket(int id)
        {
            if(id<=0) return BadRequest();
            Book book=await _context.Books.FirstOrDefaultAsync(x => x.Id==id);
            if (book is null) return NotFound();
            List<BasketCookieItemVM> basket;
            if (Request.Cookies["Basket"] is null)
            {
                basket = new List<BasketCookieItemVM>();
                BasketCookieItemVM item = new BasketCookieItemVM
                {
                    Id = id,
                    Count = 1
                };
                basket.Add(item);
            }
            else
            {
                basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);
                BasketCookieItemVM existed = basket.FirstOrDefault(x=>x.Id==id);
                if (existed is null)
                {
                    basket = new List<BasketCookieItemVM>();
                    BasketCookieItemVM item = new BasketCookieItemVM
                    {
                        Id = id,
                        Count = 1
                    };
                    basket.Add(item);
                }
                else
                {
                    existed.Count++;
                }              
            }
            string json = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("Basket", json);
            return RedirectToAction(nameof(Index), "Basket");
        }
    }
}
