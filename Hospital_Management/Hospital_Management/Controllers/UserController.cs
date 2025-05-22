using AutoMapper;
using Hospital_Management.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _http;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            IMapper mapper, IHttpContextAccessor http)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _http = http;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
