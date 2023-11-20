namespace PustokBookStore.Models
{
    public class Tags
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Booktags> Booktags {  get; set; }  
    }
}
