using KuaforDbSistemi.Data;
using KuaforDbSistemi.Models;
using Microsoft.AspNetCore.Mvc;

namespace KuaforDbSistemi.Controllers
{
    public class SalonController : Controller
    {
        private readonly KuaforContext _context;

        public SalonController(KuaforContext context)
        {
            _context = context;
        }

        // Salonların listelendiği sayfa
        public IActionResult Index()
        {
            var salonlar = _context.Salonlar?.ToList() ?? new List<Salon>();
            return View(salonlar);
        }

        // Belirli bir salonun detaylarını görüntüleme
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Salonlar == null)
            {
                return NotFound();
            }

            var salon = _context.Salonlar.FirstOrDefault(s => s.Id == id);
            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        // Yeni salon ekleme formu
        public IActionResult Create()
        {
            return View();
        }

        // Yeni salonu veritabanına kaydetme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Salon salon)
        {
            if (ModelState.IsValid)
            {
                _context.Salonlar?.Add(salon);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(salon);
        }

        // Belirli bir salonu düzenlemek için form
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Salonlar == null)
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

        // Düzenlenen salonun kaydedilmesi
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
                catch (Exception)
                {
                    if (_context.Salonlar == null || !_context.Salonlar.Any(s => s.Id == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            return View(salon);
        }

        // Belirli bir salonu silme onay sayfası
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Salonlar == null)
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

        // Salon silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Salonlar == null)
            {
                return Problem("Veritabanında 'Salonlar' tablosu bulunamadı.");
            }

            var salon = _context.Salonlar.Find(id);
            if (salon == null)
            {
                return NotFound(); // Eğer silinmek istenen salon bulunamazsa 404 döndür.
            }

            try
            {
                _context.Salonlar.Remove(salon); // Salon silme işlemi.
                _context.SaveChanges(); // Değişiklikleri kaydet.
            }
            catch (Exception ex)
            {
                // Hata durumunda bir loglama mekanizması eklenebilir.
                ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index)); // Başarılı işlem sonrası Index sayfasına yönlendir.
        }
    }
}
