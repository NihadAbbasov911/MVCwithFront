using Microsoft.AspNetCore.Mvc;

namespace MVCWFRONT.Controllers
{
    
    
        public class HomeController : Controller
        {
            public IActionResult Index()
            {
                return View();
            }

            public IActionResult Error()
            {
                return View();
            }
            public IActionResult SinglePage()
            {
                return View();
            }



    }
    }

