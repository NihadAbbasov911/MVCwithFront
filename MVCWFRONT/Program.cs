using Microsoft.EntityFrameworkCore;
using MVCWFRONT.DAL;

namespace MVCWFRONT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("Server=localhost;Database=ProniaDB;Trusted_Connection=true;Encrypt=false");
            });

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=index}"
            );


            app.Run();
        }
    }
}
