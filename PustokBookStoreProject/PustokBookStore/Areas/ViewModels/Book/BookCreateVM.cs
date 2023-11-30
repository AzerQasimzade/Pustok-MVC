using PustokBookStore.Models;

namespace PustokBookStore.Areas.ViewModels
{
    public class BookCreateVM
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Page { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Discount { get; set; }
        public bool IsDeleted { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public List<Author>? Authors { get; set; }
        public List<Tags>? Tags { get; set; }
        public List<Genre>? Genres { get; set; }
        public List<int>? TagIds { get; set; }

        public IFormFile MainPhoto { get; set; }
        public IFormFile HoverPhoto { get; set; }
        public List<IFormFile>? Photos { get; set; }


    }
}
