using KuaforDbSistemi.Data;
using KuaforDbSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KuaforDbSistemi.Controllers
{
    public class RandevuController : Controller
    {
        private readonly KuaforContext _context;

        public RandevuController(KuaforContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var randevular = _context.Randevular
                .Include(r => r.Islem)
                .Include(r => r.Calisan)
                .Include(r => r.Salon)
                .ToList();

            return View(randevular);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var randevu = _context.Randevular
                .Include(r => r.Islem)
                .Include(r => r.Calisan)
                .Include(r => r.Salon)
                .FirstOrDefault(r => r.Id == id);

            if (randevu == null)
                return NotFound();

            return View(randevu);
        }

        public IActionResult Create()
        {
            PopulateSelectLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Randevu randevu)
        {
            if (ModelState.IsValid)
            {
                if (IsRandevuAvailable(randevu))
                {
                    try
                    {
                        randevu.Durum = RandevuDurum.Beklemede;
                        _context.Randevular.Add(randevu);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException ex)
                    {
                        ModelState.AddModelError("", $"Veritabaný hatasý: {ex.InnerException?.Message ?? ex.Message}");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Seçilen saat için uygun çalýþan bulunmamaktadýr.");
                }
            }

            PopulateSelectLists(randevu);
            return View(randevu);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var randevu = _context.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Salon)
                .Include(r => r.Islem)
                .FirstOrDefault(m => m.Id == id);

            if (randevu == null)
                return NotFound();

            PopulateSelectLists(randevu);
            return View(randevu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Randevu randevu)
        {
            if (id != randevu.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevu);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Bir hata oluþtu: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            PopulateSelectLists(randevu);
            return View(randevu);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var randevu = _context.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Salon)
                .Include(r => r.Islem)
                .FirstOrDefault(m => m.Id == id);

            if (randevu == null)
                return NotFound();

            return View(randevu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var randevu = _context.Randevular.FirstOrDefault(r => r.Id == id);

            if (randevu != null)
            {
                try
                {
                    _context.Randevular.Remove(randevu);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Silme iþleminde hata oluþtu: {ex.InnerException?.Message ?? ex.Message}");
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateSelectLists(Randevu? randevu = null)
        {
            ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Isim", randevu?.SalonId);
            ViewBag.CalisanId = new SelectList(_context.Calisanlar, "Id", "Ad", randevu?.CalisanId);
            ViewBag.IslemId = new SelectList(_context.Islemler, "Id", "Ad", randevu?.IslemId);
            ViewBag.Durum = new SelectList(
                Enum.GetValues(typeof(RandevuDurum))
                    .Cast<RandevuDurum>()
                    .Select(d => new { Value = (int)d, Text = d.ToString() }),
                "Value",
                "Text",
                randevu?.Durum
            );
        }

        private bool IsRandevuAvailable(Randevu randevu)
        {
            return !_context.Randevular.Any(r =>
                r.CalisanId == randevu.CalisanId &&
                r.Tarih <= randevu.Tarih.AddMinutes(30) &&
                r.Tarih >= randevu.Tarih.AddMinutes(-30));
        }
    }
}
