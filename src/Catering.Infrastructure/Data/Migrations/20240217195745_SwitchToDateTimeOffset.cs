using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SwitchToDateTimeOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_carts_customers_customer_temp_id",
                schema: "catering",
                table: "carts");

            migrationBuilder.AddForeignKey(
                name: "fk_carts_customers_customer_id",
                schema: "catering",
                table: "carts",
                column: "customer_id",
                principalSchema: "catering",
                principalTable: "customers",
                principalColumn: "identity_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_carts_customers_customer_id",
                schema: "catering",
                table: "carts");

            migrationBuilder.AddForeignKey(
                name: "fk_carts_customers_customer_temp_id",
                schema: "catering",
                table: "carts",
                column: "customer_id",
                principalSchema: "catering",
                principalTable: "customers",
                principalColumn: "identity_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
