using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.ViewModels;
using System.ComponentModel;

namespace PustokBookStore.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public LayoutService(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }
        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            Dictionary<string, string> settings = await _context
                .Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            return settings;
        }
        public async Task<List<BasketItemVM>> GetBasketAsync()
        {
            List<BasketItemVM> items = new List<BasketItemVM>();

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
                        items.Add(item);
                    }
                }
            }        
            return items;
        }
          
    }
}

