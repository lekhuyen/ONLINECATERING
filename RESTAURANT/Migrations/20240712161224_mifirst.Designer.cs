﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RESTAURANT.API.Models;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240712161224_mifirst")]
    partial class mifirst
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RESTAURANT.API.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.CommentChild", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.ToTable("CommentChildren");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Description", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Descriptions");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MenuName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.MenuImages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagesUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("MenuImages");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<string>("RestaurantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalRating")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Restaurant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Close")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DisCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Open")
                        .HasColumnType("datetime2");

                    b.Property<int>("RatingId")
                        .HasColumnType("int");

                    b.Property<string>("RestaurantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Warning")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("RatingId")
                        .IsUnique();

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.RestaurantImages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagesUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RestaurantImages");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Comment", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Restaurant", "Restaurant")
                        .WithMany("Comment")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.CommentChild", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Comment", "Comment")
                        .WithMany("CommentChildren")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.MenuImages", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Menu", "Menu")
                        .WithMany("MenuImages")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Restaurant", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Category", "Category")
                        .WithMany("Restaurants")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RESTAURANT.API.Models.Rating", "Rating")
                        .WithOne("Restaurant")
                        .HasForeignKey("RESTAURANT.API.Models.Restaurant", "RatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Category", b =>
                {
                    b.Navigation("Restaurants");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Comment", b =>
                {
                    b.Navigation("CommentChildren");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Menu", b =>
                {
                    b.Navigation("MenuImages");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Rating", b =>
                {
                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Restaurant", b =>
                {
                    b.Navigation("Comment");
                });
#pragma warning restore 612, 618
        }
    }
}
