﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RESTAURANT.API.Models;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OrderPromotion", b =>
                {
                    b.Property<int>("OrdersId")
                        .HasColumnType("int");

                    b.Property<int>("PromotionsId")
                        .HasColumnType("int");

                    b.HasKey("OrdersId", "PromotionsId");

                    b.HasIndex("PromotionsId");

                    b.ToTable("OrderPromotions", (string)null);
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DayArrive")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Hour")
                        .HasColumnType("datetime2");

                    b.Property<int>("Member")
                        .HasColumnType("int");

                    b.Property<string>("MenuId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Pont")
                        .HasColumnType("int");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("UserId");

                    b.ToTable("Booking");
                });

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

            modelBuilder.Entity("RESTAURANT.API.Models.Combo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Combos");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.ComboDish", b =>
                {
                    b.Property<int>("DishId")
                        .HasColumnType("int");

                    b.Property<int>("ComboId")
                        .HasColumnType("int");

                    b.HasKey("DishId", "ComboId");

                    b.HasIndex("ComboId");

                    b.ToTable("ComboDishes");
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

            modelBuilder.Entity("RESTAURANT.API.Models.CustomCombo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DishId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DishId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("CustomCombos");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Description", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Descriptions");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Dish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomComboId")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Lobby", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LobbyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Lobbies");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.LobbyImages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagesUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LobbyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LobbyId");

                    b.ToTable("LobbiesImages");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Ingredient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MenuImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MenuName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

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

                    b.ToTable("MenuImages");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ComboCustomId")
                        .HasColumnType("int");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Oganization")
                        .HasColumnType("datetime2");

                    b.Property<int>("PromotionId")
                        .HasColumnType("int");

                    b.Property<int>("QuantityTable")
                        .HasColumnType("int");

                    b.Property<bool>("StatusPayment")
                        .HasColumnType("bit");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ComboCustomId")
                        .IsUnique()
                        .HasFilter("[ComboCustomId] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Promotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("QuantityTable")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

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

                    b.Property<int?>("Menu")
                        .HasColumnType("int");

                    b.Property<DateTime>("Open")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Point")
                        .HasColumnType("int");

                    b.Property<string>("RestaurantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<decimal?>("TotalRating")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("Warning")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

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

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("RestaurantImages");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("OrderPromotion", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RESTAURANT.API.Models.Promotion", null)
                        .WithMany()
                        .HasForeignKey("PromotionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Booking", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Restaurant", "Restaurant")
                        .WithMany("Bookings")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RESTAURANT.API.Models.User", "User")
                        .WithMany("Booking")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.ComboDish", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Combo", "Combo")
                        .WithMany("ComboDishes")
                        .HasForeignKey("ComboId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RESTAURANT.API.Models.Dish", "Dish")
                        .WithMany("ComboDishes")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Combo");

                    b.Navigation("Dish");
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

            modelBuilder.Entity("RESTAURANT.API.Models.CustomCombo", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Dish", "Dish")
                        .WithOne("CustomCombo")
                        .HasForeignKey("RESTAURANT.API.Models.CustomCombo", "DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RESTAURANT.API.Models.User", "User")
                        .WithOne("CustomCombo")
                        .HasForeignKey("RESTAURANT.API.Models.CustomCombo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Description", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Restaurant", "Restaurant")
                        .WithMany("Descriptions")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.LobbyImages", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Lobby", "Lobby")
                        .WithMany("LobbyImages")
                        .HasForeignKey("LobbyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lobby");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Menu", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Restaurant", "Restaurant")
                        .WithMany("Menus")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Order", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.CustomCombo", "CustomCombo")
                        .WithOne("Order")
                        .HasForeignKey("RESTAURANT.API.Models.Order", "ComboCustomId");

                    b.HasOne("RESTAURANT.API.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomCombo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Payment", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("RESTAURANT.API.Models.Payment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Rating", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Restaurant", "Restaurant")
                        .WithMany("Rating")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Restaurant", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Category", "Category")
                        .WithMany("Restaurants")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.RestaurantImages", b =>
                {
                    b.HasOne("RESTAURANT.API.Models.Restaurant", "Restaurant")
                        .WithMany("RestaurantImages")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Category", b =>
                {
                    b.Navigation("Restaurants");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Combo", b =>
                {
                    b.Navigation("ComboDishes");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Comment", b =>
                {
                    b.Navigation("CommentChildren");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.CustomCombo", b =>
                {
                    b.Navigation("Order");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Dish", b =>
                {
                    b.Navigation("ComboDishes");

                    b.Navigation("CustomCombo");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Lobby", b =>
                {
                    b.Navigation("LobbyImages");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Order", b =>
                {
                    b.Navigation("Payment");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.Restaurant", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Comment");

                    b.Navigation("Descriptions");

                    b.Navigation("Menus");

                    b.Navigation("Rating");

                    b.Navigation("RestaurantImages");
                });

            modelBuilder.Entity("RESTAURANT.API.Models.User", b =>
                {
                    b.Navigation("Booking");

                    b.Navigation("CustomCombo");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
