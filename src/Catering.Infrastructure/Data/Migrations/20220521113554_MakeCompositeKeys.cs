using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catering.Infrastructure.Data.Migrations
{
    public partial class MakeCompositeKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                schema: "catering",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemRating",
                schema: "catering",
                table: "ItemRating");

            migrationBuilder.DropIndex(
                name: "IX_ItemRating_CustomerId",
                schema: "catering",
                table: "ItemRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                schema: "catering",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "catering",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "catering",
                table: "ItemRating");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "catering",
                table: "CartItem");

            migrationBuilder.AddColumn<string>(
                name: "Contact_IdentityId",
                schema: "catering",
                table: "Menus",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                schema: "catering",
                table: "ItemRating",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                schema: "catering",
                table: "OrderItem",
                columns: new[] { "OrderId", "ItemId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemRating",
                schema: "catering",
                table: "ItemRating",
                columns: new[] { "ItemId", "CustomerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                schema: "catering",
                table: "CartItem",
                columns: new[] { "CartId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Contact_IdentityId",
                schema: "catering",
                table: "Menus",
                column: "Contact_IdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Identities_Contact_IdentityId",
                schema: "catering",
                table: "Menus",
                column: "Contact_IdentityId",
                principalSchema: "catering",
                principalTable: "Identities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Identities_Contact_IdentityId",
                schema: "catering",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                schema: "catering",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_Menus_Contact_IdentityId",
                schema: "catering",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemRating",
                schema: "catering",
                table: "ItemRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                schema: "catering",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "Contact_IdentityId",
                schema: "catering",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "catering",
                table: "OrderItem",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                schema: "catering",
                table: "ItemRating",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "catering",
                table: "ItemRating",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "catering",
                table: "CartItem",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                schema: "catering",
                table: "OrderItem",
                columns: new[] { "OrderId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemRating",
                schema: "catering",
                table: "ItemRating",
                columns: new[] { "ItemId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                schema: "catering",
                table: "CartItem",
                columns: new[] { "CartId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemRating_CustomerId",
                schema: "catering",
                table: "ItemRating",
                column: "CustomerId");
        }
    }
}
