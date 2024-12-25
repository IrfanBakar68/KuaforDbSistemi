using Microsoft.EntityFrameworkCore;
using KuaforDbSistemi.Models;

namespace KuaforDbSistemi.Data
{
    public class KuaforContext : DbContext
    {
        public KuaforContext(DbContextOptions<KuaforContext> options) : base(options) { }

        public DbSet<Salon>? Salonlar { get; set; }
        public DbSet<Islem>? Islemler { get; set; }
        public DbSet<Calisan>? Calisanlar { get; set; } // Yeni Çalışan tablosu
    }
}
