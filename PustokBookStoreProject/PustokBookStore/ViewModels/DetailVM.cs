using PustokBookStore.Models;

namespace PustokBookStore.ViewModels
{
    public class DetailVM
    {
        public Book Book { get; set; }
        public List<Book> RelatedBooks { get; set; }
        public List<Booktags> Booktags { get; set; }
        public List<Tags> Tags { get; set; }
    }
}
