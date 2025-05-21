using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
