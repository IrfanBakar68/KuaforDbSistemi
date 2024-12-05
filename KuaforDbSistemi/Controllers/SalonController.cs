using KuaforDbSistemi.Data;
using KuaforDbSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KuaforDbSistemi.Controllers
{
    public class SalonController : Controller
    {
        private readonly KuaforContext _context;

        public SalonController(KuaforContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var salonlar = _context.Salonlar
                .Include(s => s.Calisanlar)
                .Include(s => s.Islemler)
                .ToList();
            return View(salonlar);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound("Salon ID'si belirtilmedi.");
            }

            var salon = _context.Salonlar
                .Include(s => s.Calisanlar)
                .Include(s => s.Islemler)
                .FirstOrDefault(s => s.Id == id);

            if (salon == null)
            {
                return NotFound("Salon bulunamadı.");
            }

            return View(salon);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Salon salon)
        {
            if (ModelState.IsValid)
            {
                _context.Salonlar.Add(salon);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(salon);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = _context.Salonlar.Find(id);
            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Salon salon)
        {
            if (id != salon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salon);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Bir hata oluştu. Lütfen tekrar deneyiniz.");
                }
            }

            return View(salon);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = _context.Salonlar
                .Include(s => s.Calisanlar)
                .Include(s => s.Islemler)
                .FirstOrDefault(s => s.Id == id);

            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var salon = _context.Salonlar
                .Include(s => s.Calisanlar)
                .Include(s => s.Islemler)
                .FirstOrDefault(s => s.Id == id);

            if (salon != null)
            {
                if (salon.Calisanlar.Any() || _context.Randevular.Any(r => r.SalonId == salon.Id))
                {
                    ModelState.AddModelError("", "Bu salon ilişkili çalışanlar veya randevular içeriyor, silinemez.");
                    return View("Delete", salon);
                }

                _context.Salonlar.Remove(salon);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
