using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KuaforDbSistemi.Models
{
    public class Calisan
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Ad { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Soyad { get; set; }

        [MaxLength(100)]
        public string? UzmanlikAlani { get; set; }

        [MaxLength(50)]
        public string? UygunlukSaatleri { get; set; }

        public int SalonId { get; set; }
        public Salon? Salon { get; set; }

        // İlişki: Bir çalışanın birden fazla randevusu olabilir
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}
