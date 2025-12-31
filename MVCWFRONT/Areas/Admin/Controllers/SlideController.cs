using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWFRONT.DAL;
using MVCWFRONT.Models;

namespace MVCWFRONT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        public readonly AppDbContext _context;
        public SlideController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            List<Slide> slides = await _context.Sliders.ToListAsync();
            return View(slides);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<ActionResult> Create(Slide slide)
        {


            if (!ModelState.IsValid)
            {
                return View(slide);
            }

            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Fayl uygun formatda deyil");
                return View(slide);
            }

            if (slide.Photo.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Photo", "Faylin hecmi uygun deyil");
                return View(slide);
            }


            string fileName = String.Concat(Guid.NewGuid().ToString(), slide.Photo.FileName);
            string path = "C:\\Users\\user\\Desktop\\APA201\\MVCWFRONT\\MVCWFRONT\\wwwroot\\assets\\images\\website-images\\" + fileName;
            FileStream fileStream = new(path, FileMode.Create);
            await slide.Photo.CopyToAsync(fileStream);
            fileStream.Close();
            slide.Image = fileName;



            
            await _context.Sliders.AddAsync(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
