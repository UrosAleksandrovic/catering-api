using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_external",
                schema: "catering",
                table: "identities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "catering",
                table: "identities",
                keyColumn: "id",
                keyValue: "super.admin",
                columns: new[] { "is_external", "role" },
                values: new object[] { false, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_external",
                schema: "catering",
                table: "identities");

            migrationBuilder.UpdateData(
                schema: "catering",
                table: "identities",
                keyColumn: "id",
                keyValue: "super.admin",
                column: "role",
                value: 3);
        }
    }
}
