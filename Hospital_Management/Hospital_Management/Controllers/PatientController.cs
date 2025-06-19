using AutoMapper;
using Hospital_Management.DAL;
using Hospital_Management.Entities;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Hospital_Management.Controllers
{
    [Authorize]

    public class PatientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public PatientController(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, string? search = null, int take = 15)
        {
            page = page < 1 ? 1 : page;
            var query = _context.Patients.Include(x => x.AppUser)
                .Where(p => !p.IsDeleted)
                .Include(p => p.AppUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.Trim().ToLower();
                query = query.Where(p =>
                    p.AppUser.Name.ToLower().Contains(keyword) ||
                    p.AppUser.Surname.ToLower().Contains(keyword));
            }

            int totalCount = await query.CountAsync();
            double totalPage = Math.Ceiling((double)totalCount / take);

            var patients = await query
                .OrderByDescending(p => p.CreateAt)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            var vmList = _mapper.Map<List<PatientGetVM>>(patients);

            return View(new PaginationVM<PatientGetVM>
            {
                Items = vmList,
                Take = take,
                CurrentPage = page,
                TotalPage = totalPage,
                Search = search
            });
        }

        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var patient = await _context.Patients
                .Include(p => p.AppUser)
                .Include(p => p.MedicalCards)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (patient == null)
                return NotFound("Pasiyent tapılmadı.");

            var vm = _mapper.Map<PatientGetVM>(patient);
            return View(vm);
        }

        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (patient == null)
                return NotFound("Redaktə ediləcək pasiyent tapılmadı.");

            var vm = _mapper.Map<PatientUpdateVM>(patient);
            await LoadUsers();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, PatientUpdateVM vm)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            if (!ModelState.IsValid)
            {
                await LoadUsers();
                return View(vm);
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
                return NotFound("Pasiyent tapılmadı.");

            _mapper.Map(vm, patient);
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SoftDelete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
                return NotFound("Pasiyent tapılmadı.");

            patient.IsDeleted = !patient.IsDeleted;
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            TempData["Message"] = patient.IsDeleted ? "Pasiyent silindi." : "Pasiyent bərpa edildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
                return NotFound("Bazadan silinəcək pasiyent tapılmadı.");
            var user = await _userManager.FindByIdAsync(patient.AppUserId);
            if (user == null)
                return NotFound("Bazadan silinəcək User tapılmadı.");

            _context.Patients.Remove(patient);
            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Pasiyent tam silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadUsers()
        {
            ViewBag.Users = await _context.Users
                .Where(u => !_context.Patients.Any(p => p.AppUserId == u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Name + " " + u.Surname
                })
                .ToListAsync();
        }
    }

}
