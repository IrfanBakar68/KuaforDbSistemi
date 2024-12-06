using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuaforDbSistemi.Models
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MusteriAd { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string MusteriSoyad { get; set; } = string.Empty;

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public int SalonId { get; set; }

        [ForeignKey(nameof(SalonId))]
        public virtual Salon? Salon { get; set; }

        [Required]
        public int CalisanId { get; set; }

        [ForeignKey(nameof(CalisanId))]
        public virtual Calisan? Calisan { get; set; }

        [Required]
        public int IslemId { get; set; }

        [ForeignKey(nameof(IslemId))]
        public virtual Islem? Islem { get; set; }

        [Required]
        public RandevuDurum Durum { get; set; }
    }

    public enum RandevuDurum
    {
        Beklemede,
        Tamamlandi,
        IptalEdildi
    }
}
