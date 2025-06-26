using AutoMapper;
using Hospital_Management.Entities;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hospital_Management.Controllers
{
    [Authorize]

    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _http;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, IMapper mapper, 
            IHttpContextAccessor http)
        {
            _userManager = userManager;
            _mapper = mapper;
            _http = http;
        }

        public async Task<IActionResult> Index(int page = 1, string? search = null, int take = 15)
        {
            page = page < 1 ? 1 : page;
            var usersQuery = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.Trim().ToLower();
                usersQuery = usersQuery.Where(u =>
                    u.UserName.ToLower().Contains(keyword) ||
                    u.Email.ToLower().Contains(keyword) ||
                    u.Name.ToLower().Contains(keyword) ||
                    u.Surname.ToLower().Contains(keyword));
            }

            int totalCount = await usersQuery.CountAsync();
            double totalPage = Math.Ceiling((double)totalCount / take);

            var users = await usersQuery
                .OrderBy(u => u.Name)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            var vmList = _mapper.Map<List<AppUserGetVM>>(users);

            return View(new PaginationVM<AppUserGetVM>
            {
                Items = vmList,
                Take = take,
                CurrentPage = page,
                TotalPage = totalPage,
                Search = search
            });
        }

        public async Task<IActionResult> PersonalAccount(string? search = null)
        {
            // 1. Login olan istifadəçinin ID-si
            var userId = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("İstifadəçi daxil olmayıb.");

            // 2. İstifadəçini bütün əlaqələri ilə birgə query-ə daxil et
            var query = _userManager.Users
                .Include(u => u.Doctor).ThenInclude(d => d.Appointments)
                .Include(u => u.Patient).ThenInclude(p => p.MedicalCards)
                .Where(u => u.Id.Trim().ToLower().Contains(userId.Trim().ToLower()))
                .AsQueryable();

            // 3. Search varsa, tətbiq et (yalnız öz məlumatı üzərində)
            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.ToLower().Trim();
                query = query.Where(u =>
                    u.Name.ToLower().Contains(keyword) ||
                    u.Surname.ToLower().Contains(keyword) ||
                    u.UserName.ToLower().Contains(keyword) ||
                    u.Email.ToLower().Contains(keyword) ||
                    (u.Doctor != null && u.Doctor.Specialty.ToLower().Contains(keyword)) ||
                    (u.Patient != null && u.Patient.MedicalCards.Any(mc =>
                        mc.DiseaseHistory.ToLower().Contains(keyword) ||
                        mc.LabResults.ToLower().Contains(keyword)))
                );
            }

            // 4. Pagination

            var users = await query
                .OrderBy(u => u.Name)
                .ToListAsync();

            var vmList = _mapper.Map<List<AppUserGetVM>>(users);

            return View(new PaginationVM<AppUserGetVM>
            {
                Items = vmList,
                Search = search
            });
        }


        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new Exception("ID boş ola bilməz.");

            var user = await _userManager.Users
                .Include(u => u.Doctor)
                .Include(u => u.Patient)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new Exception("İstifadəçi tapılmadı.");

            var vm = _mapper.Map<AppUserGetVM>(user);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new Exception("ID boş ola bilməz.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new Exception("Silinəcək istifadəçi tapılmadı.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception("İstifadəçi silinərkən xəta baş verdi.");

            TempData["Message"] = "İstifadəçi uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new Exception("ID boş ola bilməz.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new Exception("İstifadəçi tapılmadı.");

            user.IsActivate = !user.IsActivate;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Status dəyişdirilə bilmədi.");

            TempData["Message"] = user.IsActivate ? "İstifadəçi aktiv edildi." : "İstifadəçi deaktiv edildi.";
            return RedirectToAction(nameof(Index));
        }
    }

}
