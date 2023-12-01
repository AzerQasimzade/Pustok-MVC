using PustokBookStore.Models;

namespace PustokBookStore.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Feature> Features { get; set; }
        public List<Book> Books { get; set; }
        public List<Book> DiscountBooks { get; set; }
        public List<Book> NewBooks { get; set; }

        public List<Book> ExpenciveBooks { get; set; }





    }
}
