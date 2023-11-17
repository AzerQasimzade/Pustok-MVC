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
    }
}
