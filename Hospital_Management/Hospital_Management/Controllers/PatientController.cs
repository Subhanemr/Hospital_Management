using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
