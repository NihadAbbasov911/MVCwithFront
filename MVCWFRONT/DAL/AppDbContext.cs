using Microsoft.EntityFrameworkCore;
using MVCWFRONT.Models;

namespace MVCWFRONT.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> optionsBuilder) : base(optionsBuilder)
        {

        }
        public DbSet<Slide> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Blog> Blogs { get; set; }
    }
}

