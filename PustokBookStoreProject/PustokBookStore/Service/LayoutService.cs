using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.ViewModels;
using System.ComponentModel;
using System.Net;
using System.Security.Claims;

namespace PustokBookStore.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(AppDbContext context, IHttpContextAccessor accessor,UserManager<AppUser> userManager)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
        }
        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            Dictionary<string, string> settings = await _context
                .Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            return settings;
        }
        public async Task<List<BasketItemVM>> GetBasketAsync()
        {
            List<BasketItemVM> basket = new List<BasketItemVM>();
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.Users
                   .Include(x => x.BasketItems)
                   .ThenInclude(x=>x.book)
                   .ThenInclude(b=>b.BookImages.Where(x=>x.IsPrimary==true))
                   .FirstOrDefaultAsync(x => x.Id == _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                foreach (var item in user.BasketItems)
                {
                    basket.Add(new BasketItemVM{ 
                        Name = item.book.Name,
                        Id = item.book.Id,
                        Image = item.book.BookImages.FirstOrDefault()?.Image,
                        Count = item.Count,
                        Price = item.book.CostPrice,
                        Total = item.book.CostPrice * item.Count
                    });
                }
            }
            else
            {
                if (_accessor.HttpContext.Request.Cookies["Basket"] is not null)
                {
                    List<BasketCookieItemVM> cookies = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(_accessor.HttpContext.Request.Cookies["Basket"]);
                    foreach (BasketCookieItemVM cookie in cookies)
                    {
                        Book book = await _context.Books
                            .Include(x => x.BookImages.Where(p => p.IsPrimary == true))
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
                            basket.Add(item);
                        }
                    }
                }
            }
           
            return basket;
        }
          
    }
}

