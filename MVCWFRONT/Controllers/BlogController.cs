using Microsoft.AspNetCore.Mvc;

namespace MVCWFRONT.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
