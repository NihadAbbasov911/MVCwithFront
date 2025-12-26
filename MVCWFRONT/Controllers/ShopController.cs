using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWFRONT.DAL;
using MVCWFRONT.Models;
using MVCWFRONT.ViewModels;

namespace MVCWFRONT.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Wishlist()
        {
            return View();
        }

        public IActionResult Detail(int? id)
        {
            if (id == null || id < 1)
            {
                return BadRequest();
            }

            Product product = _context.Products.Include(p => p.ProductImages).Include(p => p.Category).FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            List<Product> relatedProducts = _context.Products.Include(p=> p.ProductImages).Include(p=> p.Category).Where(p => p.CategoryId == product.CategoryId && p.Id != id).Take(8).ToList();

            DetailVM detailVM = new DetailVM
            {
                Product = product,
                RelatedProducts = relatedProducts,
            };
            return View(detailVM);
        }
    }

    }

