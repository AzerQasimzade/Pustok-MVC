using System.Net.Http.Headers;

namespace PustokBookStore.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public Book book { get; set; }
        public int bookid { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId {  get; set; }
        public int Count { get; set; }
    }
}
