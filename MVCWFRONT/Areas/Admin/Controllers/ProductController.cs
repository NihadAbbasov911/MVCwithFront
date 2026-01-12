using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWFRONT.Areas.Admin.ViewModels.Product;
using MVCWFRONT.DAL;
using MVCWFRONT.Models;

namespace MVCWFRONT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<ActionResult> Index()
        {
            List<GetProductVM> productVMs =
             await _context.Products
             .Include(p => p.Category)
             .Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
             .Select(p => new GetProductVM
             {
                 Id = p.Id,
                 Name = p.Name,
                 Price = p.Price,
                 CategoryName = p.Category.Name,
                 Image = p.ProductImages[0].Image
             })
             .ToListAsync();

            return View(productVMs);
        }

        public async Task<ActionResult> Create()
        {
            CreateProductVM createProductVM = new CreateProductVM()
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(createProductVM);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateProductVM createProductVM)
        {
            createProductVM.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(createProductVM);
            }

            bool result = await _context.Categories.AnyAsync(c => c.Id == createProductVM.CategoryId);

            if (!result)
            {
                ModelState.AddModelError(nameof(createProductVM.CategoryId), "category not found");
                return View(createProductVM);
            }

            Product product = new Product()
            {
                Name = createProductVM.Name,
                Description = createProductVM.Description,
                Price = createProductVM.Price,
                CategoryId = createProductVM.CategoryId,
                SKU = createProductVM.SKU,
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Update(int? id)
        {
            if (id == null || id < 1)
            {
                return BadRequest();
            }

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            UpdateProductVM updateProductVM = new UpdateProductVM()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                CategoryId = product.CategoryId,
                Categories = await _context.Categories.ToListAsync(),
            };
            return View(updateProductVM);
        }

        [HttpPost]
        public async Task<ActionResult> Update(int? id, UpdateProductVM updateProductVM)
        {
            if (id == null || id < 1)
            {
                return BadRequest();
            }

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            updateProductVM.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(updateProductVM);
            }

            bool result = await _context.Categories.AnyAsync(c => c.Id == updateProductVM.CategoryId);

            if (!result)
            {
                ModelState.AddModelError(nameof(Category.Id), "bele bir category yoxdur");
                return View(result);
            }

            product.Name = updateProductVM.Name;
            product.Description = updateProductVM.Description;
            product.CategoryId = updateProductVM.CategoryId;
            product.SKU = updateProductVM.SKU;
            product.Price = updateProductVM.Price;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
