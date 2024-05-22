using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TasteGuider.Models
{
    public partial class TasteGuiderDBContext : DbContext
    {
        public TasteGuiderDBContext()
        {
        }

        public TasteGuiderDBContext(DbContextOptions<TasteGuiderDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=TasteGuiderDB;Trusted_Connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ReviewText).IsUnicode(false);

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__Reviews__Restaur__4F7CD00D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Reviews__UserId__4E88ABD4");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
