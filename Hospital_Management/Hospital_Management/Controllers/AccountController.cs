using AutoMapper;
using Hospital_Management.DAL;
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
    private readonly AppDbContext _context;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        IMapper mapper, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _context = context;
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

        Doctor user = _mapper.Map<Doctor>(register);
        user.AppUser.Name = user.AppUser.Name.Capitalize();
        user.AppUser.Surname = user.AppUser.Surname.Capitalize();
        user.AppUser.DoctorId = user.Id;
        await _context.Doctors.AddAsync(user);
        var result = await _userManager.CreateAsync(user.AppUser, register.AppUser.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest("İstifadəçi yaradılarkən xəta baş verdi.");
        }

        await _userManager.AddToRoleAsync(user.AppUser, UserRoles.Doctor.ToString());
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Doctor", new { Area = "" });
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

        Patient user = _mapper.Map<Patient>(register);
        user.AppUser.Name = user.AppUser.Name.Capitalize();
        user.AppUser.Surname = user.AppUser.Surname.Capitalize();
        user.AppUser.PatientId = user.Id;
        await _context.Patients.AddAsync(user);

        var result = await _userManager.CreateAsync(user.AppUser, register.AppUser.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest("İstifadəçi yaradılarkən xəta baş verdi.");
        }

        await _userManager.AddToRoleAsync(user.AppUser, UserRoles.Patient.ToString());
        await _signInManager.SignInAsync(user.AppUser, true);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Patient", new { Area = "" });
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account", new { Area = "" });
    }
}
