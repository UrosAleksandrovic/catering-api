﻿// <auto-generated />
using System;
using Catering.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catering.Infrastructure.Data.Migrations
{
    [DbContext(typeof(CateringDbContext))]
    [Migration("20220508203246_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("catering")
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Catering.Domain.Entities.CartAggregate.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("Carts", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Entities.IdentityAggregate.Identity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Roles")
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.ToTable("Identities", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Entities.ItemAggregate.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Categories")
                        .HasColumnType("nvarchar");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Ingredients")
                        .HasColumnType("nvarchar");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(19,4)");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("Items", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Entities.MenuAggregate.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Menus", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Entities.OrderAggregate.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpectedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("Orders", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Entities.IdentityAggregate.Customer", b =>
                {
                    b.HasBaseType("Catering.Domain.Entities.IdentityAggregate.Identity");

                    b.ToTable("Customers", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Entities.IdentityAggregate.ExternalIdentity", b =>
                {
                    b.HasBaseType("Catering.Domain.Entities.IdentityAggregate.Identity");

                    b.Property<string>("_password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Password");

                    b.ToTable("ExternalIdentities", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Entities.CartAggregate.Cart", b =>
                {
                    b.HasOne("Catering.Domain.Entities.IdentityAggregate.Customer", null)
                        .WithOne()
                        .HasForeignKey("Catering.Domain.Entities.CartAggregate.Cart", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Catering.Domain.Entities.CartAggregate.CartItem", "Items", b1 =>
                        {
                            b1.Property<Guid>("CartId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("ItemId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Note")
                                .HasColumnType("text");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.HasKey("CartId", "Id");

                            b1.ToTable("CartItem", "catering");

                            b1.WithOwner()
                                .HasForeignKey("CartId");
                        });

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Catering.Domain.Entities.IdentityAggregate.Identity", b =>
                {
                    b.OwnsOne("Catering.Domain.Entities.IdentityAggregate.FullName", "FullName", b1 =>
                        {
                            b1.Property<string>("IdentityId")
                                .HasColumnType("text");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("LastName")
                                .HasColumnType("text");

                            b1.HasKey("IdentityId");

                            b1.ToTable("Identities", "catering");

                            b1.WithOwner()
                                .HasForeignKey("IdentityId");
                        });

                    b.Navigation("FullName");
                });

            modelBuilder.Entity("Catering.Domain.Entities.ItemAggregate.Item", b =>
                {
                    b.HasOne("Catering.Domain.Entities.MenuAggregate.Menu", null)
                        .WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Catering.Domain.Entities.ItemAggregate.ItemRating", "Ratings", b1 =>
                        {
                            b1.Property<Guid>("ItemId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("CustomerId")
                                .HasColumnType("text");

                            b1.Property<short>("Rating")
                                .HasColumnType("smallint");

                            b1.HasKey("ItemId", "Id");

                            b1.HasIndex("CustomerId");

                            b1.ToTable("ItemRating", "catering");

                            b1.WithOwner()
                                .HasForeignKey("ItemId");
                        });

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("Catering.Domain.Entities.MenuAggregate.Menu", b =>
                {
                    b.OwnsOne("Catering.Domain.Entities.MenuAggregate.MenuContact", "Contact", b1 =>
                        {
                            b1.Property<Guid>("MenuId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Address")
                                .HasColumnType("text");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PhoneNumber")
                                .HasColumnType("text");

                            b1.HasKey("MenuId");

                            b1.ToTable("Menus", "catering");

                            b1.WithOwner()
                                .HasForeignKey("MenuId");
                        });

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("Catering.Domain.Entities.OrderAggregate.Order", b =>
                {
                    b.OwnsOne("Catering.Domain.Entities.OrderAggregate.HomeDeliveryInfo", "HomeDeliveryInfo", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint");

                            b1.Property<string>("FloorAndAppartment")
                                .HasColumnType("text");

                            b1.Property<string>("StreetAndHouse")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders", "catering");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsMany("Catering.Domain.Entities.OrderAggregate.OrderItem", "Items", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("ItemId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Note")
                                .HasColumnType("text");

                            b1.Property<decimal>("PriceSnapshot")
                                .HasColumnType("numeric");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.HasKey("OrderId", "Id");

                            b1.ToTable("OrderItem", "catering");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("HomeDeliveryInfo");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Catering.Domain.Entities.IdentityAggregate.Customer", b =>
                {
                    b.HasOne("Catering.Domain.Entities.IdentityAggregate.Identity", null)
                        .WithOne()
                        .HasForeignKey("Catering.Domain.Entities.IdentityAggregate.Customer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Catering.Domain.Entities.IdentityAggregate.CustomerBudget", "Budget", b1 =>
                        {
                            b1.Property<string>("CustomerId")
                                .HasColumnType("text");

                            b1.Property<decimal>("Balance")
                                .HasColumnType("numeric(19,4)");

                            b1.Property<decimal>("ReservedAssets")
                                .HasColumnType("numeric(19,4)");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers", "catering");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Budget");
                });

            modelBuilder.Entity("Catering.Domain.Entities.IdentityAggregate.ExternalIdentity", b =>
                {
                    b.HasOne("Catering.Domain.Entities.IdentityAggregate.Identity", null)
                        .WithOne()
                        .HasForeignKey("Catering.Domain.Entities.IdentityAggregate.ExternalIdentity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
