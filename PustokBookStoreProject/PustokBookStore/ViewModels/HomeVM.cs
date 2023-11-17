using PustokBookStore.Models;

namespace PustokBookStore.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Feature> Features { get; set; }
        public List<Book> Books { get; set; }
        public List<BookImage> BookImages { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Author> Authors { get; set; }


    }
}
