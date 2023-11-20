namespace PustokBookStore.Models
{
    public class Booktags
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public Tags Tag  { get; set; }
        public int BookId {  get; set; }
        public Book Book { get; set; }


    }
}
