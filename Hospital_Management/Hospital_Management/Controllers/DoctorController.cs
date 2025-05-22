using AutoMapper;
using Hospital_Management.DAL;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DoctorController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1, string? search = null, int take = 15)
        {
            var query = _context.Doctors
                .Where(d => !d.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.Trim().ToLower();
                query = query.Where(d =>
                    d.Specialty.ToLower().Contains(keyword) ||
                    d.RoomNumber.ToLower().Contains(keyword) ||
                    d.WorkingHours.ToLower().Contains(keyword));
            }

            int totalCount = await query.CountAsync();
            double totalPage = Math.Ceiling((double)totalCount / take);
            if (page < 1) page = 1;
            if (page > totalPage) page = (int)totalPage;

            var doctors = await query
                .OrderBy(d => d.Specialty) // istəyə görə dəyişə bilərsən
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            var doctorVMs = _mapper.Map<List<DoctorGetVM>>(doctors);

            return View(new PaginationVM<DoctorGetVM>
            {
                Items = doctorVMs,
                Take = take,
                CurrentPage = page,
                TotalPage = totalPage,
                Search = search
            });
        }

        public async Task<IActionResult> GetById(string id)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (doctor == null) return NotFound();

            var vm = _mapper.Map<DoctorGetVM>(doctor);
            return View(vm);
        }
        public async Task<IActionResult> Update(string id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null) return NotFound();

            var vm = _mapper.Map<DoctorUpdateVM>(doctor);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, DoctorUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm); // FluentValidation avtomatik işləyir
            }

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (id != doctor!.Id) return BadRequest();

            if (doctor == null) return NotFound();

            _mapper.Map(vm, doctor);

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> SoftDelete(string id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null) return NotFound();

            doctor.IsDeleted = !doctor.IsDeleted; // Ters çevir: true ↔ false

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            TempData["Message"] = doctor.IsDeleted ? "Həkim silindi." : "Həkim bərpa edildi.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null) return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Həkim bir başa silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
