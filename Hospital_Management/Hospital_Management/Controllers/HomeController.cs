using Hospital_Management.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {


            return View();
        }

        public IActionResult ErrorPage(string error)
        {
            if (error == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model: error);
        }
    }
}
