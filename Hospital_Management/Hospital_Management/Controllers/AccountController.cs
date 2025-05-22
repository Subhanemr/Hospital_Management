using AutoMapper;
using Hospital_Management.Entities;
using Hospital_Management.Enums;
using Hospital_Management.Extantions;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    [AllowAnonymous]
    public async  Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM login)
    {
        if (!ModelState.IsValid)
            return View(login);

        AppUser? user = await _userManager.FindByNameAsync(login.UserNameOrEmail);

        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(login.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "İstifadəçi adı, email və ya şifrə yanlışdır.");
                return BadRequest("İstifadəçi tapılmadı.");
            }
        }

        if (user.IsActivate == true)
        {
            ModelState.AddModelError(string.Empty, "Hesabınız aktiv deyil.");
            return BadRequest("İstifadəçi aktiv deyil.");
        }

        var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsRemembered, true);

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Hesabınız bloklanıb. Zəhmət olmasa gözləyin.");
            return BadRequest("Hesab bloklanıb.");
        }

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "İstifadəçi adı, email və ya şifrə yanlışdır.");
            return BadRequest("Şifrə və ya istifadəçi yanlışdır.");
        }

        return RedirectToAction("Index", "Home", new { Area = "" });
    }
    [Authorize]

    public async Task<IActionResult> RegisterDoctor()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterDoctor(DoctorCreateVM register)
    {
        if (!ModelState.IsValid)
            return BadRequest("Məlumatlar düzgün deyil.");

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
            return BadRequest("İstifadəçi yaradılarkən xəta baş verdi.");
        }

        await _userManager.AddToRoleAsync(user, UserRoles.Doctor.ToString());

        return RedirectToAction("Index", "Home", new { Area = "" });
    }

    public async Task<IActionResult> RegisterPatient()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterPatient(PatientCreateVM register)
    {
        if (!ModelState.IsValid)
            return BadRequest("Məlumatlar düzgün daxil edilməyib.");

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
            return BadRequest("İstifadəçi yaradılarkən xəta baş verdi.");
        }

        await _userManager.AddToRoleAsync(user, UserRoles.Patient.ToString());
        await _signInManager.SignInAsync(user, true);

        return RedirectToAction("Index", "Home", new { Area = "" });
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account", new { Area = "" });
    }
}
