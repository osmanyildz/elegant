﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ecommerce.data.Concrete.EfCore;

#nullable disable

namespace ecommerce.data.Migrations
{
    [DbContext(typeof(ECommerceContext))]
    [Migration("20231206080801_ReNameSubCatgTableField")]
    partial class ReNameSubCatgTableField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("ecommerce.entity.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("ecommerce.entity.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CartId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SizeType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("ecommerce.entity.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ParentCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ecommerce.entity.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("ecommerce.entity.GenderCategory", b =>
                {
                    b.Property<int>("GenderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GenderId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("GenderCategories");
                });

            modelBuilder.Entity("ecommerce.entity.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ecommerce.entity.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConversationId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OrderState")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PaymentId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ecommerce.entity.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("ecommerce.entity.ParentCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ParentCategories");
                });

            modelBuilder.Entity("ecommerce.entity.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("GenderId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPopular")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ParentCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ecommerce.entity.ProductCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CategoryId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("ecommerce.entity.ProductSizeType", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SizeTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductId", "SizeTypeId");

                    b.HasIndex("SizeTypeId");

                    b.ToTable("ProductSizeTypes");
                });

            modelBuilder.Entity("ecommerce.entity.SizeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ParentCategoryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("SizeTypes");
                });

            modelBuilder.Entity("ecommerce.entity.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("subCategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("ecommerce.entity.CartItem", b =>
                {
                    b.HasOne("ecommerce.entity.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ecommerce.entity.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ecommerce.entity.Category", b =>
                {
                    b.HasOne("ecommerce.entity.ParentCategory", "ParentCategory")
                        .WithMany("Categories")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("ecommerce.entity.GenderCategory", b =>
                {
                    b.HasOne("ecommerce.entity.Category", "Category")
                        .WithMany("GenderCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ecommerce.entity.Gender", "Gender")
                        .WithMany("GenderCategories")
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Gender");
                });

            modelBuilder.Entity("ecommerce.entity.Image", b =>
                {
                    b.HasOne("ecommerce.entity.Product", "Product")
                        .WithMany("ImageUrls")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ecommerce.entity.OrderItem", b =>
                {
                    b.HasOne("ecommerce.entity.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ecommerce.entity.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ecommerce.entity.Product", b =>
                {
                    b.HasOne("ecommerce.entity.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ecommerce.entity.ParentCategory", "ParentCategory")
                        .WithMany("Products")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gender");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("ecommerce.entity.ProductCategory", b =>
                {
                    b.HasOne("ecommerce.entity.Category", "Category")
                        .WithMany("ProductCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ecommerce.entity.Product", "Product")
                        .WithMany("ProductCategories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ecommerce.entity.ProductSizeType", b =>
                {
                    b.HasOne("ecommerce.entity.Product", "Product")
                        .WithMany("ProductSizeTypes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ecommerce.entity.SizeType", "SizeType")
                        .WithMany("ProductSizeTypes")
                        .HasForeignKey("SizeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("SizeType");
                });

            modelBuilder.Entity("ecommerce.entity.SizeType", b =>
                {
                    b.HasOne("ecommerce.entity.ParentCategory", "ParentCategory")
                        .WithMany("SizeTypes")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("ecommerce.entity.SubCategory", b =>
                {
                    b.HasOne("ecommerce.entity.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ecommerce.entity.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("ecommerce.entity.Category", b =>
                {
                    b.Navigation("GenderCategories");

                    b.Navigation("ProductCategories");
                });

            modelBuilder.Entity("ecommerce.entity.Gender", b =>
                {
                    b.Navigation("GenderCategories");
                });

            modelBuilder.Entity("ecommerce.entity.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("ecommerce.entity.ParentCategory", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Products");

                    b.Navigation("SizeTypes");
                });

            modelBuilder.Entity("ecommerce.entity.Product", b =>
                {
                    b.Navigation("ImageUrls");

                    b.Navigation("ProductCategories");

                    b.Navigation("ProductSizeTypes");
                });

            modelBuilder.Entity("ecommerce.entity.SizeType", b =>
                {
                    b.Navigation("ProductSizeTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
