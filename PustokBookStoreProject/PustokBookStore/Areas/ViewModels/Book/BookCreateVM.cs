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

    }
}
