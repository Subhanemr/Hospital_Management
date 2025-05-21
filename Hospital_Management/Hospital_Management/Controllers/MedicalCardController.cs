using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class MedicalCardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
