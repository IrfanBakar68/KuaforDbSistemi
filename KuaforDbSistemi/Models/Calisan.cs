using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuaforDbSistemi.Models
{
    public class Calisan
    {
        [Key]
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

        [ForeignKey("Salon")]
        public int SalonId { get; set; }

        public Salon? Salon { get; set; } // Null atanabilir olarak işaretlendi
    }
}
