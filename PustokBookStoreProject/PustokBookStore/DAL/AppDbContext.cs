using Microsoft.EntityFrameworkCore;
using PustokBookStore.Models;

namespace PustokBookStore.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Booktags> BookTags { get; set; }
        public DbSet<Tags> Tags { get; set; }

        public DbSet<Setting> Settings { get; set; }







    }
}
