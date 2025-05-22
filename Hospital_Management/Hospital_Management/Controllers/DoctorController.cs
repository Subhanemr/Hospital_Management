using AutoMapper;
using Hospital_Management.DAL;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Controllers
{
    [Authorize]

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
            page = page < 1 ? 1 : page;
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
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (doctor == null)
                return NotFound("Göstərilən ID-yə uyğun həkim tapılmadı.");

            var vm = _mapper.Map<DoctorGetVM>(doctor);
            return View(vm);
        }

        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
                return NotFound("Redaktə ediləcək həkim tapılmadı.");

            var vm = _mapper.Map<DoctorUpdateVM>(doctor);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, DoctorUpdateVM vm)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
                return NotFound("Həkim tapılmadı.");

            _mapper.Map(vm, doctor);

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SoftDelete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
                return NotFound("Silinəcək həkim tapılmadı.");

            doctor.IsDeleted = !doctor.IsDeleted;

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            TempData["Message"] = doctor.IsDeleted ? "Həkim silindi." : "Həkim bərpa edildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
                return NotFound("Bazadan silinəcək həkim tapılmadı.");

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Həkim bir başa silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
