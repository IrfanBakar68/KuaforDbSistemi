using System;
using System.ComponentModel.DataAnnotations;

namespace KuaforDbSistemi.Models
{
    public enum RandevuDurum
    {
        Beklemede,
        Onaylandi,
        IptalEdildi
    }

    public class Randevu
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Müşteri adı zorunludur.")]
        [MaxLength(100, ErrorMessage = "Müşteri adı 100 karakterden fazla olamaz.")]
        public string MusteriAd { get; set; } = string.Empty;

        [Required(ErrorMessage = "Müşteri soyadı zorunludur.")]
        [MaxLength(100, ErrorMessage = "Müşteri soyadı 100 karakterden fazla olamaz.")]
        public string MusteriSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Randevu tarihi zorunludur.")]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "İşlem seçimi zorunludur.")]
        public int IslemId { get; set; }
        public Islem? Islem { get; set; }

        [Required(ErrorMessage = "Çalışan seçimi zorunludur.")]
        public int CalisanId { get; set; }
        public Calisan? Calisan { get; set; }

        [Required(ErrorMessage = "Salon seçimi zorunludur.")]
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }

        [Required(ErrorMessage = "Randevu durumu zorunludur.")]
        public RandevuDurum Durum { get; set; } = RandevuDurum.Beklemede;
    }
}
