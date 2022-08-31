using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Data.Migrations
{
    public partial class RefactorCategoriesAndIngredients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                schema: "catering",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Ingredients",
                schema: "catering",
                table: "Items");

            migrationBuilder.CreateTable(
                name: "ItemCategory",
                schema: "catering",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategory", x => new { x.ItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_ItemCategory_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "catering",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemIngredient",
                schema: "catering",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemIngredient", x => new { x.ItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_ItemIngredient_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "catering",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCategory",
                schema: "catering");

            migrationBuilder.DropTable(
                name: "ItemIngredient",
                schema: "catering");

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                schema: "catering",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ingredients",
                schema: "catering",
                table: "Items",
                type: "text",
                nullable: true);
        }
    }
}
