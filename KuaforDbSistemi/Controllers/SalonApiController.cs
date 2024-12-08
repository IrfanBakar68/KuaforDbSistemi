using KuaforDbSistemi.Data;
using KuaforDbSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SalonApiController : ControllerBase
{
    private readonly KuaforContext _context;

    public SalonApiController(KuaforContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tüm salonları getirir.
    /// </summary>
    /// <returns>Salon listesi</returns>
    [HttpGet]
    public IActionResult GetAllSalons()
    {
        var salons = _context.Salonlar.ToList();
        return Ok(salons);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip salonu getirir.
    /// </summary>
    /// <param name="id">Salon ID</param>
    /// <returns>Salon bilgisi</returns>
    [HttpGet("{id}")]
    public IActionResult GetSalonById(int id)
    {
        var salon = _context.Salonlar.FirstOrDefault(s => s.Id == id);
        if (salon == null)
        {
            return NotFound(new { Message = "Salon bulunamadı." });
        }
        return Ok(salon);
    }

    /// <summary>
    /// Yeni bir salon ekler.
    /// </summary>
    /// <param name="salon">Salon nesnesi</param>
    /// <returns>Eklenen salon</returns>
    [HttpPost]
    public IActionResult AddSalon(Salon salon)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _context.Salonlar.Add(salon);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetSalonById), new { id = salon.Id }, salon);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { Message = "Bir hata oluştu.", Detail = ex.InnerException?.Message });
        }
    }

    /// <summary>
    /// Belirtilen ID'ye sahip salonu günceller.
    /// </summary>
    /// <param name="id">Salon ID</param>
    /// <param name="salon">Güncellenmiş salon bilgisi</param>
    /// <returns>Güncellenen salon</returns>
    [HttpPut("{id}")]
    public IActionResult UpdateSalon(int id, Salon salon)
    {
        if (id != salon.Id)
        {
            return BadRequest(new { Message = "ID uyumsuzluğu." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _context.Entry(salon).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { Message = "Bir hata oluştu.", Detail = ex.InnerException?.Message });
        }
    }

    /// <summary>
    /// Belirtilen ID'ye sahip salonu siler.
    /// </summary>
    /// <param name="id">Salon ID</param>
    /// <returns>Silme işlemi sonucu</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteSalon(int id)
    {
        var salon = _context.Salonlar
            .Include(s => s.Calisanlar)
            .FirstOrDefault(s => s.Id == id);

        if (salon == null)
        {
            return NotFound(new { Message = "Salon bulunamadı." });
        }

        if (salon.Calisanlar.Any())
        {
            return BadRequest(new { Message = "Bu salon ilişkilendirilmiş çalışanlara sahip. Önce bu çalışanları kaldırın." });
        }

        try
        {
            _context.Salonlar.Remove(salon);
            _context.SaveChanges();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { Message = "Silme işleminde bir hata oluştu.", Detail = ex.InnerException?.Message });
        }
    }
}
