using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Mailing.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mailing");

            migrationBuilder.CreateTable(
                name: "failed_emails",
                schema: "mailing",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    recepiants = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: false),
                    generated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_failed_emails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "templates",
                schema: "mailing",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    html_template = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_templates", x => x.name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "failed_emails",
                schema: "mailing");

            migrationBuilder.DropTable(
                name: "templates",
                schema: "mailing");
        }
    }
}
