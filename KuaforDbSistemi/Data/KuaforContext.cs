using Microsoft.EntityFrameworkCore;
using KuaforDbSistemi.Models;

namespace KuaforDbSistemi.Data
{
    public class KuaforContext : DbContext
    {
        public KuaforContext(DbContextOptions<KuaforContext> options) : base(options) { }

        public DbSet<Salon> Salonlar { get; set; } = null!;
        public DbSet<Islem> Islemler { get; set; } = null!;
        public DbSet<Calisan> Calisanlar { get; set; } = null!;
        public DbSet<Randevu> Randevular { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Salon ve Çalışan ilişkisi (Salon birden fazla Çalışan'a sahip olabilir)
            modelBuilder.Entity<Salon>()
                .HasMany(s => s.Calisanlar)
                .WithOne(c => c.Salon)
                .HasForeignKey(c => c.SalonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Salon ve İşlem ilişkisi (Salon birden fazla İşlem'e sahip olabilir)
            modelBuilder.Entity<Salon>()
                .HasMany(s => s.Islemler)
                .WithOne(i => i.Salon)
                .HasForeignKey(i => i.SalonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Randevu ve Çalışan ilişkisi
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Calisan)
                .WithMany()
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Restrict); // Silme sırasında kısıtlama

            // Randevu ve İşlem ilişkisi
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Islem)
                .WithMany()
                .HasForeignKey(r => r.IslemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Randevu ve Salon ilişkisi
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Salon)
                .WithMany()
                .HasForeignKey(r => r.SalonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
