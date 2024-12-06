using KuaforDbSistemi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KuaforDbSistemi.Data
{
    public class KuaforContext : IdentityDbContext<IdentityUser>
    {
        public KuaforContext(DbContextOptions<KuaforContext> options) : base(options)
        {
        }

        public DbSet<Salon> Salonlar { get; set; } = null!;
        public DbSet<Islem> Islemler { get; set; } = null!;
        public DbSet<Calisan> Calisanlar { get; set; } = null!;
        public DbSet<Randevu> Randevular { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Salon>()
                .HasMany(s => s.Calisanlar)
                .WithOne(c => c.Salon)
                .HasForeignKey(c => c.SalonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Salon>()
                .HasMany(s => s.Islemler)
                .WithOne(i => i.Salon)
                .HasForeignKey(i => i.SalonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Calisan)
                .WithMany()
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Islem)
                .WithMany()
                .HasForeignKey(r => r.IslemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Salon)
                .WithMany()
                .HasForeignKey(r => r.SalonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
