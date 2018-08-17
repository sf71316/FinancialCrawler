using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Financial.Data.Models
{
    public partial class FinancialContext : DbContext
    {

        public FinancialContext(DbContextOptions<FinancialContext> options)
            : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<CurrencyHistory> CurrencyHistory { get; set; }
        public virtual DbSet<CurrencyNameRelation> CurrencyNameRelation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencySymbol)
                    .IsRequired()
                    .HasColumnName("currencySymbol")
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<CurrencyHistory>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .ValueGeneratedNever();

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("decimal(18,6)");
            });

            modelBuilder.Entity<CurrencyNameRelation>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("Currency_Name_Relation");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CurrencyUid).HasColumnName("CurrencyUID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
