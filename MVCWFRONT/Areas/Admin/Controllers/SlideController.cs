using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWFRONT.Areas.Admin.ViewModels.Slide;
using MVCWFRONT.DAL;
using MVCWFRONT.Models;
using MVCWFRONT.Utilities.Enums;
using MVCWFRONT.Utilities.Extensions;

namespace MVCWFRONT.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //public class SlideController : Controller
    //{
    //    public readonly AppDbContext _context;
    //    public SlideController(AppDbContext context)
    //    {
    //        _context = context;
    //    }
    //    public async Task<ActionResult> Index()
    //    {
    //        List<Slide> slides = await _context.Sliders.ToListAsync();
    //        return View(slides);
    //    }

    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    [HttpPost]

    //    public async Task<ActionResult> Create(Slide slide)
    //    {


    //        if (!ModelState.IsValid)
    //        {
    //            return View(slide);
    //        }

    //        if (!slide.Photo.ContentType.Contains("image/"))
    //        {
    //            ModelState.AddModelError("Photo", "Fayl uygun formatda deyil");
    //            return View(slide);
    //        }

    //        if (slide.Photo.Length > 2 * 1024 * 1024)
    //        {
    //            ModelState.AddModelError("Photo", "Faylin hecmi uygun deyil");
    //            return View(slide);
    //        }


    //        string fileName = String.Concat(Guid.NewGuid().ToString(), slide.Photo.FileName);
    //        string path = "C:\\Users\\user\\Desktop\\APA201\\MVCWFRONT\\MVCWFRONT\\wwwroot\\assets\\images\\website-images\\" + fileName;
    //        FileStream fileStream = new(path, FileMode.Create);
    //        await slide.Photo.CopyToAsync(fileStream);
    //        fileStream.Close();
    //        slide.Image = fileName;




    //        await _context.Sliders.AddAsync(slide);
    //        await _context.SaveChangesAsync();

    //        return RedirectToAction("Index");
    //    }
    //}
    [Area("Admin")]

    public class SlideController : Controller

    {
        public readonly AppDbContext _context;
        public readonly IWebHostEnvironment _env;
        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        public async Task<ActionResult> Create(CreateSlideVM createSlideVM)
        {


            if (!ModelState.IsValid)
            {
                return View();
            }
            // slide.Photo.ValidateType("image/")
            if (!createSlideVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "Siz uygun formatta file elave etmirsiz");
                return View();
            }
            // file, fileSizeType, size 
            // slide.Pthoto, kb,mg,gb, 5

            if (createSlideVM.Photo.ValidateSize(FileSize.MB, 20))
            {
                ModelState.AddModelError("Photo", "Siz duzgun hecmde file elave etmirsiz");
                return View();
            }


            Slide slide = new Slide()
            {
                Title = createSlideVM.Title,
                Description = createSlideVM.Description,
                Discount = createSlideVM.Discount,
                Order = createSlideVM.Order,
                Image = await createSlideVM.Photo.CreateFile(_env.WebRootPath, "assets", "images", "website-images")
            };



            // return Content(slide.Photo.FileName + " " + slide.Photo.ContentType + " " + slide.Photo.Length);
            await _context.Sliders.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id < 1)
            {
                return BadRequest();
            }

            Slide slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null)
            {
                return NotFound();
            }

            // System.IO.File.Delete(Path.Combine(_env.WebRootPath,"assets","images","website-images",slide.Image));

            slide.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
            _context.Sliders.Remove(slide);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public async Task<ActionResult> Update(int? id)
        {
            if (id == null || id < 1)
            {
                return BadRequest();
            }

            Slide slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null)
            {
                return NotFound();
            }

            UpdateSlideVM updateSlideVM = new UpdateSlideVM()
            {
                Title = slide.Title,
                Description = slide.Description,
                Discount = slide.Discount,
                Order = slide.Order,
                Image = slide.Image,
            };
            return View(updateSlideVM);
        }

        [HttpPost]
        public async Task<ActionResult> Update(int? id, UpdateSlideVM updateSlideVM)
        {
            if (!ModelState.IsValid)
            {
                return View(updateSlideVM);
            }

            Slide slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (updateSlideVM.Photo is not null)
            {
                if (!updateSlideVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(updateSlideVM.Photo), "invalid type");
                    return View(updateSlideVM);
                }

                if (updateSlideVM.Photo.ValidateSize(FileSize.KB, 20))
                {
                    ModelState.AddModelError(nameof(updateSlideVM.Photo), "invalid size");
                    return View(updateSlideVM);
                }

                string filename = await updateSlideVM.Photo.CreateFile(_env.WebRootPath, "assets", "images", "website-images");
                slide.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
                slide.Image = filename;
            }


            slide.Title = updateSlideVM.Title;
            slide.Description = updateSlideVM.Description;
            slide.Discount = updateSlideVM.Discount;
            slide.Order = updateSlideVM.Order;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
