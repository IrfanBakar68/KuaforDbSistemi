namespace KuaforDbSistemi.Models
{
    public class Islem
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public double Ucret { get; set; }
        public int Sure { get; set; } // Dakika olarak süre
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }
    }
}
