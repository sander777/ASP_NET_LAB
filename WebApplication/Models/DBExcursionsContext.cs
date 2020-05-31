using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication
{
    public partial class DBExcursionsContext : DbContext
    {
        public DBExcursionsContext()
        {
        }

        public DBExcursionsContext(DbContextOptions<DBExcursionsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ExcurionGuide> ExcurionGuide { get; set; }
        public virtual DbSet<Excursion> Excursion { get; set; }
        public virtual DbSet<Guide> Guide { get; set; }
        public virtual DbSet<Pattern> Pattern { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DBExcursions; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Salary).HasColumnType("money");
            });

            modelBuilder.Entity<ExcurionGuide>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idguide).HasColumnName("IDGuide");

                entity.Property(e => e.Idpattern).HasColumnName("IDPattern");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.IdguideNavigation)
                    .WithMany(p => p.ExcurionGuide)
                    .HasForeignKey(d => d.Idguide)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExcurionGuide_Guide");

                entity.HasOne(d => d.IdpatternNavigation)
                    .WithMany(p => p.ExcurionGuide)
                    .HasForeignKey(d => d.Idpattern)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExcurionGuide_Pattern");
            });

            modelBuilder.Entity<Excursion>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.IdexcursionGuide)
                    .HasColumnName("IDExcursionGuide");

                entity.HasOne(d => d.IdexcursionGuideNavigation)
                    .WithMany(p => p.Excursion)
                    .HasForeignKey(d => d.IdexcursionGuide)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Excursion_ExcurionGuide");
            });

            modelBuilder.Entity<Guide>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Guide)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guide_Category");
            });

            modelBuilder.Entity<Pattern>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
