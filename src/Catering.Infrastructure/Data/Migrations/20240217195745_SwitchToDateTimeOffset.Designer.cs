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
    [Migration("20240217195745_SwitchToDateTimeOffset")]
    partial class SwitchToDateTimeOffset
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("catering")
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Catering.Domain.Aggregates.Cart.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("customer_id");

                    b.Property<Guid?>("MenuId")
                        .HasColumnType("uuid")
                        .HasColumnName("menu_id");

                    b.HasKey("Id")
                        .HasName("pk_carts");

                    b.HasIndex("CustomerId")
                        .IsUnique()
                        .HasDatabaseName("ix_carts_customer_id");

                    b.ToTable("carts", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Expense.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("CustomerId")
                        .HasColumnType("text")
                        .HasColumnName("customer_id");

                    b.Property<DateTimeOffset>("DeliveredOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("delivered_on");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid")
                        .HasColumnName("menu_id");

                    b.Property<string>("Note")
                        .HasColumnType("text")
                        .HasColumnName("note");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.HasKey("Id")
                        .HasName("pk_expenses");

                    b.ToTable("expenses", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Identity.Customer", b =>
                {
                    b.Property<string>("IdentityId")
                        .HasColumnType("text")
                        .HasColumnName("identity_id");

                    b.HasKey("IdentityId")
                        .HasName("pk_customers");

                    b.ToTable("customers", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Identity.Identity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.ToTable("identities", "catering");

                    b.UseTptMappingStrategy();

                    b.HasData(
                        new
                        {
                            Id = "super.admin",
                            Email = "super.admin@catering.test",
                            Role = 3
                        });
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Identity.IdentityInvitation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTimeOffset>("ExpiredOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expired_on");

                    b.Property<int>("FutureRole")
                        .HasColumnType("integer")
                        .HasColumnName("future_role");

                    b.Property<bool>("IsCustomer")
                        .HasColumnType("boolean")
                        .HasColumnName("is_customer");

                    b.HasKey("Id")
                        .HasName("pk_identity_invitations");

                    b.ToTable("identity_invitations", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Item.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid")
                        .HasColumnName("menu_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("DECIMAL(19,4)")
                        .HasColumnName("price");

                    b.HasKey("Id")
                        .HasName("pk_items");

                    b.HasIndex("MenuId")
                        .HasDatabaseName("ix_items_menu_id");

                    b.ToTable("items", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Menu.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_menus");

                    b.ToTable("menus", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Order.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("customer_id");

                    b.Property<DateTimeOffset>("ExpectedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expected_on");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid")
                        .HasColumnName("menu_id");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.ToTable("orders", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Identity.CateringIdentity", b =>
                {
                    b.HasBaseType("Catering.Domain.Aggregates.Identity.Identity");

                    b.Property<string>("_password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.ToTable("catering_identities", "catering");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Cart.Cart", b =>
                {
                    b.HasOne("Catering.Domain.Aggregates.Identity.Customer", null)
                        .WithOne()
                        .HasForeignKey("Catering.Domain.Aggregates.Cart.Cart", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_carts_customers_customer_id");

                    b.OwnsMany("Catering.Domain.Aggregates.Cart.CartItem", "Items", b1 =>
                        {
                            b1.Property<Guid>("CartId")
                                .HasColumnType("uuid")
                                .HasColumnName("cart_id");

                            b1.Property<Guid>("ItemId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid")
                                .HasColumnName("item_id");

                            b1.Property<string>("Note")
                                .HasColumnType("text")
                                .HasColumnName("note");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer")
                                .HasColumnName("quantity");

                            b1.HasKey("CartId", "ItemId")
                                .HasName("pk_cart_item");

                            b1.ToTable("cart_item", "catering");

                            b1.WithOwner()
                                .HasForeignKey("CartId")
                                .HasConstraintName("fk_cart_item_carts_cart_id");
                        });

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Identity.Customer", b =>
                {
                    b.HasOne("Catering.Domain.Aggregates.Identity.Identity", "Identity")
                        .WithOne()
                        .HasForeignKey("Catering.Domain.Aggregates.Identity.Customer", "IdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_customers_identities_identity_id");

                    b.OwnsOne("Catering.Domain.Aggregates.Identity.CustomerBudget", "Budget", b1 =>
                        {
                            b1.Property<string>("CustomerIdentityId")
                                .HasColumnType("text")
                                .HasColumnName("identity_id");

                            b1.Property<decimal>("Balance")
                                .HasColumnType("decimal(19,4)")
                                .HasColumnName("budget_balance");

                            b1.Property<decimal>("ReservedAssets")
                                .HasColumnType("decimal(19,4)")
                                .HasColumnName("budget_reserved_assets");

                            b1.HasKey("CustomerIdentityId");

                            b1.ToTable("customers", "catering");

                            b1.WithOwner()
                                .HasForeignKey("CustomerIdentityId")
                                .HasConstraintName("fk_customers_customers_identity_id");
                        });

                    b.Navigation("Budget");

                    b.Navigation("Identity");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Identity.Identity", b =>
                {
                    b.OwnsOne("Catering.Domain.Aggregates.Identity.FullName", "FullName", b1 =>
                        {
                            b1.Property<string>("IdentityId")
                                .HasColumnType("text")
                                .HasColumnName("id");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("full_name_first_name");

                            b1.Property<string>("LastName")
                                .HasColumnType("text")
                                .HasColumnName("full_name_last_name");

                            b1.HasKey("IdentityId");

                            b1.ToTable("identities", "catering");

                            b1.WithOwner()
                                .HasForeignKey("IdentityId")
                                .HasConstraintName("fk_identities_identities_id");
                        });

                    b.Navigation("FullName");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Identity.IdentityInvitation", b =>
                {
                    b.OwnsOne("Catering.Domain.Aggregates.Identity.FullName", "FullName", b1 =>
                        {
                            b1.Property<string>("IdentityInvitationId")
                                .HasColumnType("text")
                                .HasColumnName("id");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("full_name_first_name");

                            b1.Property<string>("LastName")
                                .HasColumnType("text")
                                .HasColumnName("full_name_last_name");

                            b1.HasKey("IdentityInvitationId");

                            b1.ToTable("identity_invitations", "catering");

                            b1.WithOwner()
                                .HasForeignKey("IdentityInvitationId")
                                .HasConstraintName("fk_identity_invitations_identity_invitations_id");
                        });

                    b.Navigation("FullName");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Item.Item", b =>
                {
                    b.HasOne("Catering.Domain.Aggregates.Menu.Menu", null)
                        .WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_items_menus_menu_id");

                    b.OwnsMany("Catering.Domain.Aggregates.Item.ItemCategory", "Categories", b1 =>
                        {
                            b1.Property<Guid>("ItemId")
                                .HasColumnType("uuid")
                                .HasColumnName("item_id");

                            b1.Property<string>("Id")
                                .HasColumnType("text")
                                .HasColumnName("id");

                            b1.Property<string>("DisplayName")
                                .HasColumnType("text")
                                .HasColumnName("display_name");

                            b1.HasKey("ItemId", "Id")
                                .HasName("pk_item_category");

                            b1.ToTable("item_category", "catering");

                            b1.WithOwner()
                                .HasForeignKey("ItemId")
                                .HasConstraintName("fk_item_category_items_item_id");
                        });

                    b.OwnsMany("Catering.Domain.Aggregates.Item.ItemIngredient", "Ingredients", b1 =>
                        {
                            b1.Property<Guid>("ItemId")
                                .HasColumnType("uuid")
                                .HasColumnName("item_id");

                            b1.Property<string>("Id")
                                .HasColumnType("text")
                                .HasColumnName("id");

                            b1.Property<string>("DisplayName")
                                .HasColumnType("text")
                                .HasColumnName("display_name");

                            b1.HasKey("ItemId", "Id")
                                .HasName("pk_item_ingredient");

                            b1.ToTable("item_ingredient", "catering");

                            b1.WithOwner()
                                .HasForeignKey("ItemId")
                                .HasConstraintName("fk_item_ingredient_items_item_id");
                        });

                    b.OwnsMany("Catering.Domain.Aggregates.Item.ItemRating", "Ratings", b1 =>
                        {
                            b1.Property<Guid>("ItemId")
                                .HasColumnType("uuid")
                                .HasColumnName("item_id");

                            b1.Property<string>("CustomerId")
                                .HasColumnType("text")
                                .HasColumnName("customer_id");

                            b1.Property<short>("Rating")
                                .HasColumnType("smallint")
                                .HasColumnName("rating");

                            b1.HasKey("ItemId", "CustomerId")
                                .HasName("pk_item_rating");

                            b1.ToTable("item_rating", "catering");

                            b1.WithOwner()
                                .HasForeignKey("ItemId")
                                .HasConstraintName("fk_item_rating_items_item_id");
                        });

                    b.Navigation("Categories");

                    b.Navigation("Ingredients");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Menu.Menu", b =>
                {
                    b.OwnsOne("Catering.Domain.Aggregates.Menu.MenuContact", "Contact", b1 =>
                        {
                            b1.Property<Guid>("MenuId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("Address")
                                .HasColumnType("text")
                                .HasColumnName("contact_address");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("contact_email");

                            b1.Property<string>("IdentityId")
                                .HasColumnType("text")
                                .HasColumnName("contact_identity_id");

                            b1.Property<string>("PhoneNumber")
                                .HasColumnType("text")
                                .HasColumnName("contact_phone_number");

                            b1.HasKey("MenuId");

                            b1.HasIndex("IdentityId")
                                .HasDatabaseName("ix_menus_contact_identity_id");

                            b1.ToTable("menus", "catering");

                            b1.HasOne("Catering.Domain.Aggregates.Identity.Identity", null)
                                .WithMany()
                                .HasForeignKey("IdentityId")
                                .HasConstraintName("fk_menus_identities_contact_identity_id");

                            b1.WithOwner()
                                .HasForeignKey("MenuId")
                                .HasConstraintName("fk_menus_menus_id");
                        });

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Order.Order", b =>
                {
                    b.OwnsOne("Catering.Domain.Aggregates.Order.HomeDeliveryInfo", "HomeDeliveryInfo", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("FloorAndAppartment")
                                .HasColumnType("text")
                                .HasColumnName("home_delivery_info_floor_and_appartment");

                            b1.Property<string>("StreetAndHouse")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("home_delivery_info_street_and_house");

                            b1.HasKey("OrderId");

                            b1.ToTable("orders", "catering");

                            b1.WithOwner()
                                .HasForeignKey("OrderId")
                                .HasConstraintName("fk_orders_orders_id");
                        });

                    b.OwnsMany("Catering.Domain.Aggregates.Order.OrderItem", "Items", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint")
                                .HasColumnName("order_id");

                            b1.Property<Guid>("ItemId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid")
                                .HasColumnName("item_id");

                            b1.Property<string>("NameSnapshot")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("name_snapshot");

                            b1.Property<string>("Note")
                                .HasColumnType("text")
                                .HasColumnName("note");

                            b1.Property<decimal>("PriceSnapshot")
                                .HasColumnType("numeric")
                                .HasColumnName("price_snapshot");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer")
                                .HasColumnName("quantity");

                            b1.HasKey("OrderId", "ItemId")
                                .HasName("pk_order_item");

                            b1.ToTable("order_item", "catering");

                            b1.WithOwner()
                                .HasForeignKey("OrderId")
                                .HasConstraintName("fk_order_item_orders_order_id");
                        });

                    b.Navigation("HomeDeliveryInfo");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Catering.Domain.Aggregates.Identity.CateringIdentity", b =>
                {
                    b.HasOne("Catering.Domain.Aggregates.Identity.Identity", null)
                        .WithOne()
                        .HasForeignKey("Catering.Domain.Aggregates.Identity.CateringIdentity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_catering_identities_identities_id");
                });
#pragma warning restore 612, 618
        }
    }
}
