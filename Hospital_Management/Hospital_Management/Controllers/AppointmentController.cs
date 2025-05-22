using AutoMapper;
using Hospital_Management.DAL;
using Hospital_Management.Entities;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Controllers
{
    [Authorize]

    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1, string? search = null, int take = 15)
        {
            page = page < 1 ? 1 : page;
            var query = _context.Appointments
                .Where(a => !a.IsDeleted)
                .Include(a => a.Doctor)
                .Include(a => a.Patient).ThenInclude(p => p.AppUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.ToLower().Trim();
                query = query.Where(a =>
                    a.Status.ToLower().Contains(keyword) ||
                    a.Doctor.Specialty.ToLower().Contains(keyword) ||
                    a.Patient.AppUser.Name.ToLower().Contains(keyword));
            }

            int totalCount = await query.CountAsync();
            double totalPage = Math.Ceiling((double)totalCount / take);

            var appointments = await query
                .OrderByDescending(a => a.AppointmentDate)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            var vmList = _mapper.Map<List<AppointmentGetVM>>(appointments);

            return View(new PaginationVM<AppointmentGetVM>
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

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient).ThenInclude(p => p.AppUser)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

            if (appointment == null)
                return NotFound("Belə bir görüş tapılmadı.");

            var vm = _mapper.Map<AppointmentGetVM>(appointment);
            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            await LoadDoctorAndPatientViewBags();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDoctorAndPatientViewBags();
                return View(vm);
            }

            var entity = _mapper.Map<Appointment>(vm);

            await _context.Appointments.AddAsync(entity);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Yeni görüş uğurla yaradıldı.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            if (appointment == null)
                return NotFound("Redaktə ediləcək görüş tapılmadı.");

            var vm = _mapper.Map<AppointmentUpdateVM>(appointment);
            await LoadDoctorAndPatientViewBags();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, AppointmentUpdateVM vm)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            if (!ModelState.IsValid)
            {
                await LoadDoctorAndPatientViewBags();
                return View(vm);
            }

            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
                return NotFound("Görüş tapılmadı.");

            _mapper.Map(vm, appointment);
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SoftDelete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
                return NotFound("Silinəcək görüş tapılmadı.");

            appointment.IsDeleted = !appointment.IsDeleted;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Görüş uğurla silindi (soft delete).";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
                return NotFound("Silinəcək görüş bazada tapılmadı.");

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Görüş tam olaraq silindi (hard delete).";
            return RedirectToAction(nameof(Index));
        }

        // Helper method for ViewBag
        private async Task LoadDoctorAndPatientViewBags()
        {
            ViewBag.Doctors = await _context.Doctors
                .Where(d => !d.IsDeleted)
                .Select(d => new SelectListItem { Value = d.Id, Text = d.Specialty })
                .ToListAsync();

            ViewBag.Patients = await _context.Patients
                .Where(p => !p.IsDeleted)
                .Include(p => p.AppUser)
                .Select(p => new SelectListItem { Value = p.Id, Text = p.AppUser.Name + " " + p.AppUser.Surname })
                .ToListAsync();
        }
    }

}
