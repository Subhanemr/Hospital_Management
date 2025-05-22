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
    public class MedicalCardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MedicalCardController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1, string? search = null, int take = 15)
        {
            page = page < 1 ? 1 : page;
            var query = _context.MedicalCards
                .Where(mc => !mc.IsDeleted)
                .Include(mc => mc.Patient).ThenInclude(p => p.AppUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.Trim().ToLower();
                query = query.Where(mc =>
                    mc.DiseaseHistory.ToLower().Contains(keyword) ||
                    mc.LabResults.ToLower().Contains(keyword) ||
                    mc.Patient.AppUser.Name.ToLower().Contains(keyword));
            }

            int totalCount = await query.CountAsync();
            double totalPage = Math.Ceiling((double)totalCount / take);

            var cards = await query
                .OrderByDescending(mc => mc.CreateAt)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            var vmList = _mapper.Map<List<MedicalCardGetVM>>(cards);

            return View(new PaginationVM<MedicalCardGetVM>
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

            var card = await _context.MedicalCards
                .Include(mc => mc.Patient).ThenInclude(p => p.AppUser)
                .FirstOrDefaultAsync(mc => mc.Id == id && !mc.IsDeleted);

            if (card == null) return NotFound();

            var vm = _mapper.Map<MedicalCardGetVM>(card);
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            await LoadPatients();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MedicalCardCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadPatients();
                return View(vm);
            }

            var entity = _mapper.Map<MedicalCard>(vm);

            await _context.MedicalCards.AddAsync(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var card = await _context.MedicalCards.FirstOrDefaultAsync(mc => mc.Id == id && !mc.IsDeleted);
            if (card == null) return NotFound();

            var vm = _mapper.Map<MedicalCardUpdateVM>(card);
            await LoadPatients();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, MedicalCardUpdateVM vm)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            if (!ModelState.IsValid)
            {
                await LoadPatients();
                return View(vm);
            }

            var card = await _context.MedicalCards.FirstOrDefaultAsync(mc => mc.Id == id);
            if (card == null) return NotFound();

            _mapper.Map(vm, card);
            _context.MedicalCards.Update(card);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SoftDelete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var card = await _context.MedicalCards.FirstOrDefaultAsync(mc => mc.Id == id);
            if (card == null) return NotFound();

            card.IsDeleted = !card.IsDeleted;
            _context.MedicalCards.Update(card);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Tibbi kart soft silindi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID boş ola bilməz.");

            var card = await _context.MedicalCards.FirstOrDefaultAsync(mc => mc.Id == id);
            if (card == null) return NotFound();

            _context.MedicalCards.Remove(card);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Tibbi kart tam silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadPatients()
        {
            ViewBag.Patients = await _context.Patients
                .Where(p => !p.IsDeleted)
                .Include(p => p.AppUser)
                .Select(p => new SelectListItem
                {
                    Value = p.Id,
                    Text = p.AppUser.Name + " " + p.AppUser.Surname
                })
                .ToListAsync();
        }
    }

}
