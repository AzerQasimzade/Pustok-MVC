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
            List<BasketCookieItemVM> basket=new List<BasketCookieItemVM>();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.Users
                    .Include(x => x.BasketItems)
                    .FirstOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (user is null) return NotFound();
                var basketItem = user.BasketItems.FirstOrDefault(x => x.bookid == id);
                if (basketItem is null)
                {
                    user.BasketItems.Add(new BasketItem
                    {
                        bookid = book.Id,
                        Count = 1,

                    });
                }
                else
                {
                    basketItem.Count++;
                }
                await _context.SaveChangesAsync();


                foreach (var dbitem in user.BasketItems)
                {
                    Book newbook = await _context.Books
                        .Include(x => x.BookImages.Where(p => p.IsPrimary == true))
                        .FirstOrDefaultAsync(x => x.Id == dbitem.bookid);


                    if (newbook is not null)
                    {
                        BasketItemVM basitem = new BasketItemVM
                        {
                            Name = newbook.Name,
                            Id = newbook.Id,
                            Image = newbook.BookImages.FirstOrDefault().Image,
                            Count = dbitem.Count,
                            Price = newbook.CostPrice,
                            Total = newbook.CostPrice * dbitem.Count,
                        };
                    }
                }
                return View();
            }
           
            else
            {
                string json = Request.Cookies["Basket"];
                List<BasketItemVM> cookieVMs= new List<BasketItemVM>();
                BasketItemVM item = null;
                if (!string.IsNullOrEmpty(json))
                {
                    cookieVMs=JsonConvert.DeserializeObject<List<BasketItemVM>>(json);
                    item=cookieVMs.FirstOrDefault(x => x.Id == id);
                }
                if (item != null)
                {
                    item.Count++;
                }
                else
                {
                    BasketItemVM cookieVM = new BasketItemVM()
                    {
                        Id = id,
                        Count = 1,
                    };
                    cookieVMs.Add(cookieVM);
                }
                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieVMs));

                foreach (var cookie in cookieVMs)
                {
                    Book newbook = await _context.Books
                        .Include(x => x.BookImages.Where(p => p.IsPrimary == true))
                        .FirstOrDefaultAsync(x => x.Id == cookie.Id);

                    if (newbook is not null)
                    {
                        BasketItemVM basitem = new BasketItemVM
                        {
                            Name = newbook.Name,
                            Id = newbook.Id,
                            Image = newbook.BookImages.FirstOrDefault().Image,
                            Count = cookie.Count,
                            Price = newbook.CostPrice,
                            Total = newbook.CostPrice * cookie.Count,
                        };
                    }
                }
                return PartialView("BasketPartial", basket);

            }
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
