using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace randevuOtomasyonu.Models;

public partial class RandevuprojeContext : DbContext
{
    public RandevuprojeContext()
    {
    }

    public RandevuprojeContext(DbContextOptions<RandevuprojeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Musteri> Musteris { get; set; }

    public virtual DbSet<Uygulamalar> Uygulamalars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=projerandevu.cbeacicsksgh.us-east-1.rds.amazonaws.com,1433;Database=randevuproje;User ID=admin;Password=12345678;Trusted_Connection=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Login>(entity =>
        {
            entity.ToTable("login");

            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Sifre).HasMaxLength(50);
        });

        modelBuilder.Entity<Musteri>(entity =>
        {
            entity.ToTable("Musteri");

            entity.Property(e => e.MusteriId).HasColumnName("MusteriID");
            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Marka).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.MotorTipi).HasMaxLength(50);
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.Telefon)
                .HasMaxLength(11)
                .IsFixedLength();
        });

        modelBuilder.Entity<Uygulamalar>(entity =>
        {
            entity.HasKey(e => e.ServisId);

            entity.ToTable("Uygulamalar");

            entity.Property(e => e.ServisId).HasColumnName("ServisID");
            entity.Property(e => e.ServisTuru).HasMaxLength(50);
            entity.Property(e => e.ServisUcreti).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
