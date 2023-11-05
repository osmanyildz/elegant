using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.data.Concrete.EfCore
{
    public class ECommerceContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ParentCategory> ParentCategories { get; set; }
        public DbSet<SizeType> SizeTypes { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSizeType> ProductSizeTypes { get; set; }
        public DbSet<GenderCategory> GenderCategories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source = ecommerceDb");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GenderCategory>().HasKey(g=>new{g.GenderId,g.CategoryId});
            modelBuilder.Entity<ProductCategory>().HasKey(p=> new{p.CategoryId,p.ProductId});
            modelBuilder.Entity<ProductSizeType>().HasKey(p=>new{p.ProductId,p.SizeTypeId});
        }
    }
}