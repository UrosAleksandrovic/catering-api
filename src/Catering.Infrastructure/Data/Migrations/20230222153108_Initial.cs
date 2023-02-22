using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catering.Infrastructure.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catering");

            migrationBuilder.CreateTable(
                name: "identities",
                schema: "catering",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    full_name_first_name = table.Column<string>(type: "text", nullable: true),
                    full_name_last_name = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "catering",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<string>(type: "text", nullable: false),
                    expected_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<byte>(type: "smallint", nullable: false),
                    home_delivery_info_street_and_house = table.Column<string>(type: "text", nullable: true),
                    home_delivery_info_floor_and_appartment = table.Column<string>(type: "text", nullable: true),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "catering_identities",
                schema: "catering",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catering_identities", x => x.id);
                    table.ForeignKey(
                        name: "fk_catering_identities_identities_id",
                        column: x => x.id,
                        principalSchema: "catering",
                        principalTable: "identities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "catering",
                columns: table => new
                {
                    identity_id = table.Column<string>(type: "text", nullable: false),
                    budget_balance = table.Column<decimal>(type: "numeric(19,4)", nullable: true),
                    budget_reserved_assets = table.Column<decimal>(type: "numeric(19,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.identity_id);
                    table.ForeignKey(
                        name: "fk_customers_identities_identity_id",
                        column: x => x.identity_id,
                        principalSchema: "catering",
                        principalTable: "identities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menus",
                schema: "catering",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    contact_phone_number = table.Column<string>(type: "text", nullable: true),
                    contact_email = table.Column<string>(type: "text", nullable: true),
                    contact_address = table.Column<string>(type: "text", nullable: true),
                    contact_identity_id = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_menus", x => x.id);
                    table.ForeignKey(
                        name: "fk_menus_identities_contact_identity_id",
                        column: x => x.contact_identity_id,
                        principalSchema: "catering",
                        principalTable: "identities",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                schema: "catering",
                columns: table => new
                {
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_snapshot = table.Column<string>(type: "text", nullable: false),
                    price_snapshot = table.Column<decimal>(type: "numeric", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_item", x => new { x.order_id, x.item_id });
                    table.ForeignKey(
                        name: "fk_order_item_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "catering",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "carts",
                schema: "catering",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_carts", x => x.id);
                    table.ForeignKey(
                        name: "fk_carts_customers_customer_temp_id",
                        column: x => x.customer_id,
                        principalSchema: "catering",
                        principalTable: "customers",
                        principalColumn: "identity_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "items",
                schema: "catering",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric(19,4)", nullable: false),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_items_menus_menu_id",
                        column: x => x.menu_id,
                        principalSchema: "catering",
                        principalTable: "menus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart_item",
                schema: "catering",
                columns: table => new
                {
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cart_item", x => new { x.cart_id, x.item_id });
                    table.ForeignKey(
                        name: "fk_cart_item_carts_cart_id",
                        column: x => x.cart_id,
                        principalSchema: "catering",
                        principalTable: "carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_category",
                schema: "catering",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_category", x => new { x.item_id, x.id });
                    table.ForeignKey(
                        name: "fk_item_category_items_item_id",
                        column: x => x.item_id,
                        principalSchema: "catering",
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_ingredient",
                schema: "catering",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_ingredient", x => new { x.item_id, x.id });
                    table.ForeignKey(
                        name: "fk_item_ingredient_items_item_id",
                        column: x => x.item_id,
                        principalSchema: "catering",
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_rating",
                schema: "catering",
                columns: table => new
                {
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_rating", x => new { x.item_id, x.customer_id });
                    table.ForeignKey(
                        name: "fk_item_rating_items_item_id",
                        column: x => x.item_id,
                        principalSchema: "catering",
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "catering",
                table: "identities",
                columns: new[] { "id", "email", "role" },
                values: new object[] { "super.admin", "super.admin@catering.test", 3 });

            migrationBuilder.CreateIndex(
                name: "ix_carts_customer_id",
                schema: "catering",
                table: "carts",
                column: "customer_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_items_menu_id",
                schema: "catering",
                table: "items",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "ix_menus_contact_identity_id",
                schema: "catering",
                table: "menus",
                column: "contact_identity_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_item",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "catering_identities",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "item_category",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "item_ingredient",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "item_rating",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "order_item",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "carts",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "items",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "customers",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "menus",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "identities",
                schema: "catering");
        }
    }
}
