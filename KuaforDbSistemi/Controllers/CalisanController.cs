using KuaforDbSistemi.Data;
using KuaforDbSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KuaforDbSistemi.Controllers
{
    public class CalisanController : Controller
    {
        private readonly KuaforContext _context;

        public CalisanController(KuaforContext context)
        {
            _context = context;
        }

        // Çalýþanlarýn listelendiði sayfa
        public IActionResult Index()
        {
            var calisanlar = _context.Calisanlar?.Include(c => c.Salon).ToList() ?? new List<Calisan>();
            return View(calisanlar);
        }

        // Çalýþan detaylarýnýn görüntülendiði sayfa
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound("Çalýþan ID'si belirtilmedi.");
            }

            var calisan = _context.Calisanlar?.Include(c => c.Salon).FirstOrDefault(m => m.Id == id);
            if (calisan == null)
            {
                return NotFound("Çalýþan bulunamadý.");
            }

            return View(calisan);
        }

        // Yeni çalýþan ekleme formu
        public IActionResult Create()
        {
            ViewBag.SalonId = new SelectList(_context.Salonlar ?? Enumerable.Empty<Salon>(), "Id", "Isim");
            return View();
        }

        // Yeni çalýþanýn veritabanýna kaydedilmesi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Calisan calisan)
        {
            if (_context.Salonlar == null || !_context.Salonlar.Any(s => s.Id == calisan.SalonId))
            {
                ModelState.AddModelError("SalonId", "Geçersiz bir salon seçildi.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Calisanlar?.Add(calisan);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Bir hata oluþtu: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            ViewBag.SalonId = new SelectList(_context.Salonlar ?? Enumerable.Empty<Salon>(), "Id", "Isim", calisan.SalonId);
            return View(calisan);
        }

        // Çalýþan düzenleme formu
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("Çalýþan ID'si belirtilmedi.");
            }

            var calisan = _context.Calisanlar?.Find(id);
            if (calisan == null)
            {
                return NotFound("Çalýþan bulunamadý.");
            }

            ViewBag.SalonId = new SelectList(_context.Salonlar ?? Enumerable.Empty<Salon>(), "Id", "Isim", calisan.SalonId);
            return View(calisan);
        }

        // Düzenlenen çalýþanýn kaydedilmesi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Calisan calisan)
        {
            if (id != calisan.Id)
            {
                return NotFound("Çalýþan ID'si uyuþmuyor.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calisan);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Bir hata oluþtu: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            ViewBag.SalonId = new SelectList(_context.Salonlar ?? Enumerable.Empty<Salon>(), "Id", "Isim", calisan.SalonId);
            return View(calisan);
        }

        // Çalýþan silme onay sayfasý
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Çalýþan ID'si belirtilmedi.");
            }

            var calisan = _context.Calisanlar?.Include(c => c.Salon).FirstOrDefault(m => m.Id == id);
            if (calisan == null)
            {
                return NotFound("Çalýþan bulunamadý.");
            }

            return View(calisan);
        }

        // Çalýþan silme iþlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Calisanlar == null)
            {
                return Problem("Veritabanýnda 'Calisanlar' tablosu bulunamadý.");
            }

            var calisan = _context.Calisanlar.Find(id);
            if (calisan != null)
            {
                try
                {
                    _context.Calisanlar.Remove(calisan);
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Silme iþleminde bir hata oluþtu: {ex.InnerException?.Message ?? ex.Message}");
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
