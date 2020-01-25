using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PizzaBox.Domain.Models
{
    public partial class PizzaDbContext : DbContext
    {
        public PizzaDbContext()
        {
        }

        public PizzaDbContext(DbContextOptions<PizzaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrder { get; set; }
        public virtual DbSet<Store> Store { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-UQJVL69;Database=PizzaDb;trusted_connection= True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__Customer__536C85E54F61C403");

                entity.ToTable("Customer", "Revature");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Passw)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Previousorder)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__Customer__0809335DD3873DD9");

                entity.ToTable("CustomerOrder", "Revature");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.Pizza)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 9)");

                entity.Property(e => e.Storename)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.StorenameNavigation)
                    .WithMany(p => p.CustomerOrder)
                    .HasForeignKey(d => d.Storename)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Store_Order_Name");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.CustomerOrder)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Order_Name");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Storename)
                    .HasName("PK__Store__0BACB3FE7F6D864C");

                entity.ToTable("Store", "Revature");

                entity.Property(e => e.Storename)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Storepassword)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
