namespace milkTea.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MilkTeaModel : DbContext
    {
        public MilkTeaModel()
            : base("name=MilkTeaModels")
        {
        }

        public virtual DbSet<Acc_Sell_Pro> Acc_Sell_Pro { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<product_detail> product_detail { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<User_Accounts> User_Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acc_Sell_Pro>()
                .Property(e => e.Username)
                .IsFixedLength();

            modelBuilder.Entity<Cart>()
                .Property(e => e.Username)
                .IsFixedLength();

            modelBuilder.Entity<product_detail>()
                .Property(e => e.SizeName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Imgage_url)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Desciption)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Acc_Sell_Pro)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.product_detail)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Size>()
                .Property(e => e.SizeName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Size>()
                .HasMany(e => e.product_detail)
                .WithRequired(e => e.Size)
                .WillCascadeOnDelete(false);

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
                .HasMany(e => e.Acc_Sell_Pro)
                .WithRequired(e => e.User_Accounts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Accounts>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.User_Accounts)
                .WillCascadeOnDelete(false);
        }
    }
}
