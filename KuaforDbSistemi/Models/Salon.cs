using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KuaforDbSistemi.Models
{
    public class Salon
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Isim { get; set; }

        public string? Adres { get; set; }
        public string? CalismaSaatleri { get; set; }

        // İlişkiler
        public ICollection<Calisan> Calisanlar { get; set; } = new List<Calisan>();
        public ICollection<Islem> Islemler { get; set; } = new List<Islem>();
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>(); // Randevular ile ilişki eklendi
    }
}
