using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace milkTea.Models
{
    public partial class MilkTeaModel : DbContext
    {
        public MilkTeaModel()
            : base("name=MilkTeaModel")
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Orders_Detail> Orders_Detail { get; set; }
        public virtual DbSet<Products_Detail> Products_Detail { get; set; }
        public virtual DbSet<Ship_Method> Ship_Method { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<User_Accounts> User_Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .Property(e => e.Username)
                .IsFixedLength();

            modelBuilder.Entity<Orders_Detail>()
                .Property(e => e.Customer)
                .IsFixedLength();

            modelBuilder.Entity<Orders_Detail>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Orders_Detail1)
                .HasForeignKey(e => e.Orders_Detail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products_Detail>()
                .Property(e => e.Seller)
                .IsFixedLength();

            modelBuilder.Entity<Products_Detail>()
                .Property(e => e.Imgage_url)
                .IsUnicode(false);

            modelBuilder.Entity<Products_Detail>()
                .Property(e => e.Size)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Products_Detail>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Products_Detail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Size>()
                .Property(e => e.SizeName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Size>()
                .HasMany(e => e.Products_Detail)
                .WithOptional(e => e.Size1)
                .HasForeignKey(e => e.Size);

            modelBuilder.Entity<User_Accounts>()
                .Property(e => e.Username)
                .IsFixedLength();

            modelBuilder.Entity<User_Accounts>()
                .Property(e => e.Password)
                .IsFixedLength();

            modelBuilder.Entity<User_Accounts>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<User_Accounts>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<User_Accounts>()
                .Property(e => e.Avatar_url)
                .IsUnicode(false);

            modelBuilder.Entity<User_Accounts>()
                .HasMany(e => e.Orders_Detail)
                .WithRequired(e => e.User_Accounts)
                .HasForeignKey(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Accounts>()
                .HasMany(e => e.Products_Detail)
                .WithOptional(e => e.User_Accounts)
                .HasForeignKey(e => e.Seller);
        }
    }
}
