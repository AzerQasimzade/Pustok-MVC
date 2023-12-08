using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.ViewModels;
using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace PustokBookStore.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                        .Include(x => x.Booktags)
                        .ThenInclude(x => x.Tag)
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
            if (id <= 0) return BadRequest();
            Book book = await _context
                .Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book is null) return NotFound();
            List<BasketCookieItemVM> basket;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.Users
                    .Include(x => x.BasketItems)
                    .FirstOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (user is null) return NotFound();
                BasketItem item = new BasketItem
                {
                    AppUserId = user.Id,
                    bookid = book.Id,
                    Count = 1,
                    Price = book.CostPrice,
                };
                foreach (var basitem in user.BasketItems)
                {
                    Book book1 = await _context.Books
                        .Include(p => p.BookImages.Where(bi => bi.IsPrimary == true)).FirstOrDefaultAsync(b => b.Id == item.Id);
                    if (book1 != null)
                    {
                        BasketItemVM basketBookVM=new BasketItemVM
                        {

                        }
                    }
                }
                
            }
            else
            {
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
                    BasketCookieItemVM existed = basket.FirstOrDefault(x => x.Id == id);
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
                

            }

            string json = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("Basket", json);



            List<BasketItemVM> items = new List<BasketItemVM>();

                foreach (var cookie in basket)
                {
                    Book newbook = await _context.Books
                        .Include(x => x.BookImages.Where(p => p.IsPrimary == true))
                        .FirstOrDefaultAsync(x => x.Id == cookie.Id);

                    if (newbook is not null)
                    {
                        BasketItemVM item = new BasketItemVM
                        {
                            Name = newbook.Name,
                            Id = newbook.Id,
                            Image = newbook.BookImages.FirstOrDefault().Image,
                            Count = cookie.Count,
                            Price = newbook.CostPrice,
                            Total = newbook.CostPrice * cookie.Count,
                        };
                        items.Add(item);
                    }
                }
            return PartialView("BasketPartial", items);
        }
        public async Task<IActionResult> RemoveBasket(int id)
        {
            if (id <= 0) return BadRequest();
            Book book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book is null) return NotFound();
            List<BasketCookieItemVM> basket;
            basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);
            BasketCookieItemVM existed = basket.FirstOrDefault(x => x.Id == id);
            basket.Remove(existed);
            string json = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("Basket", json);
            return RedirectToAction(nameof(Index), "Basket");
        }
        public async Task<IActionResult> PlusBasket(int id)
        {
            if (id <= 0) return BadRequest();
            Book book = await _context.Books.FirstOrDefaultAsync(p => p.Id == id);
            if (book is null) return NotFound();
            List<BasketCookieItemVM> basket;
            basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);
            BasketCookieItemVM existed = basket.FirstOrDefault(x => x.Id == id);
            if (existed is not null)
            {
                basket.FirstOrDefault(x => x.Id == id).Count++;
            }
            string json = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("Basket", json);
            return RedirectToAction(nameof(Index), "Basket");
        }
        public async Task<IActionResult> MinusBasket(int id)
        {
            if (id <= 0) return BadRequest();
            Book book = await _context.Books.FirstOrDefaultAsync(p => p.Id == id);
            if (book is null) return NotFound();
            List<BasketCookieItemVM> basket;
            basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(Request.Cookies["Basket"]);
            BasketCookieItemVM existed = basket.FirstOrDefault(x => x.Id == id);
            if (existed is not null)
            {
                basket.FirstOrDefault(x => x.Id == id).Count--;

                if (basket.FirstOrDefault(x => x.Id == id).Count == 0)
                {
                    basket.Remove(existed);
                }
            }
            string json = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("Basket", json);
            return RedirectToAction(nameof(Index), "Basket");
        }
    }
}
