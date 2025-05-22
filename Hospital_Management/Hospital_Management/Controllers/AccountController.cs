using AutoMapper;
using Hospital_Management.Entities;
using Hospital_Management.Enums;
using Hospital_Management.Exceptions;
using Hospital_Management.Extantions;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _http;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            IMapper mapper, IHttpContextAccessor http)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _http = http;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
                throw new InvalidInputException("");

            AppUser user = await _userManager.FindByNameAsync(login.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(login.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username, Email or Password is wrong");
                    throw new LoginException("");
                }
            }
            if (user.IsActivate == true)
            {
                ModelState.AddModelError(string.Empty, "Your account is not active");
                throw new LoginException("");
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsRemembered, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your Account is locked-out please wait");
                throw new LoginException("");
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username, Email or Password is wrong");
                throw new LoginException("");
            }

            _http.HttpContext.Response.Cookies.Delete("FavoriteEstate");

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public IActionResult RegisterDoctor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(DoctorCreateVM register)
        {
            if (!ModelState.IsValid) 
                throw new InvalidInputException("");

            AppUser user = _mapper.Map<AppUser>(register.AppUser);
            user.Name = user.Name.Capitalize();
            user.Surname = user.Surname.Capitalize();

            var result = await _userManager.CreateAsync(user, register.AppUser.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                throw new LoginException("");
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Doctor.ToString());

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public IActionResult RegisterPatient()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterPatient(PatientCreateVM register)
        {
            if (!ModelState.IsValid)
                throw new InvalidInputException("");

            AppUser user = _mapper.Map<AppUser>(register.AppUser);
            user.Name = user.Name.Capitalize();
            user.Surname = user.Surname.Capitalize();

            var result = await _userManager.CreateAsync(user, register.AppUser.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                throw new LoginException("");
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Patient.ToString());
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}
