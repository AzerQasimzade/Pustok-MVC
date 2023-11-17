namespace PustokBookStore.Models
{
    public class BookImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool? IsPrimary { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

    }
}
