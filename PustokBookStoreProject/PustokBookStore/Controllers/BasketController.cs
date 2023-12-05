using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.ViewModels;
using System.Drawing;
using System.Net.Http.Headers;

namespace PustokBookStore.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> items = new List<BasketItemVM>();
            if (Request.Cookies["Basket"] is not null)
            {
                List<BasketCookieItemVM> cookies = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);
                foreach (BasketCookieItemVM cookie in cookies)
                {
                    Book book = await _context.Books
                        .Include(x => x.BookImages.Where(p => p.IsPrimary == true))
                        .Include(x=>x.Booktags)
                        .ThenInclude(x=>x.Tag)
                        .FirstOrDefaultAsync(x => x.Id == cookie.Id);
                    if (book is not null)
                    {
                        BasketItemVM item = new BasketItemVM
                        {
                            Name = book.Name,
                            Id = book.Id,
                            Image = book.BookImages.FirstOrDefault().Image,
                            Count = cookie.Count,
                            Price = book.CostPrice,
                            Total = book.CostPrice * cookie.Count,
                        };
                        items.Add(item);
                    }
                }
            }                
            return View(items);
        }
        public async Task<IActionResult> AddBasket(int id)
        {
            if(id<=0) return BadRequest();
            Book book=await _context
                .Books.FirstOrDefaultAsync(x => x.Id==id);
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
        public async Task<IActionResult> RemoveBasket(int id)
        {
			if (id <= 0) return BadRequest();
			Book book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
			if (book is null) return NotFound();
			List<BasketCookieItemVM> basket;
            basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);
			BasketCookieItemVM existed =basket.FirstOrDefault(x=>x.Id==id);
            basket.Remove(existed);
            string json= JsonConvert.SerializeObject(basket);
			Response.Cookies.Append("Basket", json);
			return RedirectToAction(nameof(Index), "Basket");



		}
    }
}
