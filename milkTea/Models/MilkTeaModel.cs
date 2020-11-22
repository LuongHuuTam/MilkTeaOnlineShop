using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace milkTea.Models
{
    public partial class MilkTeaModel : DbContext
    {
        public MilkTeaModel()
            : base("name=MilkTeaModels")
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<DeliveryProduct> DeliveryProducts { get; set; }
        public virtual DbSet<Products_detail> Products_detail { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<User_Accounts> User_Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .Property(e => e.Username)
                .IsFixedLength();

            modelBuilder.Entity<DeliveryProduct>()
                .Property(e => e.Seller)
                .IsFixedLength();

            modelBuilder.Entity<DeliveryProduct>()
                .Property(e => e.Customer)
                .IsFixedLength();

            modelBuilder.Entity<Products_detail>()
                .Property(e => e.Seller)
                .IsFixedLength();

            modelBuilder.Entity<Products_detail>()
                .Property(e => e.Imgage_url)
                .IsUnicode(false);

            modelBuilder.Entity<Products_detail>()
                .Property(e => e.Size)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Products_detail>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.Products_detail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products_detail>()
                .HasMany(e => e.DeliveryProducts)
                .WithRequired(e => e.Products_detail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Size>()
                .Property(e => e.SizeName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Size>()
                .HasMany(e => e.Products_detail)
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
                .HasMany(e => e.Carts)
                .WithRequired(e => e.User_Accounts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Accounts>()
                .HasMany(e => e.DeliveryProducts)
                .WithRequired(e => e.User_Accounts)
                .HasForeignKey(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Accounts>()
                .HasMany(e => e.DeliveryProducts1)
                .WithRequired(e => e.User_Accounts1)
                .HasForeignKey(e => e.Seller)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Accounts>()
                .HasMany(e => e.Products_detail)
                .WithOptional(e => e.User_Accounts)
                .HasForeignKey(e => e.Seller);
        }
    }
}
