using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _2C2P.Assessment.DataLayer.Models;

public partial class AssessmentDbContext : DbContext
{
    public AssessmentDbContext()
    {
    }

    public AssessmentDbContext(DbContextOptions<AssessmentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ImportedDatum> ImportedData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=FIRDHAUS-PC\\SQLEXPRESS19;User ID=sa;Password=P@ssw0rd;Database=AssessmentDB;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImportedDatum>(entity =>
        {
            entity.HasKey(e => e.TxId);

            entity.Property(e => e.TxId)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FinalStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SourceData)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TxDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
