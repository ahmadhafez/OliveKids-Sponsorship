using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OliveKids.Migrations
{
    public partial class SposorProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Sponsor",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Sponsor",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Sponsor",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Receipt",
                table: "Sponsor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Sponsor");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Sponsor");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Sponsor");

            migrationBuilder.DropColumn(
                name: "Receipt",
                table: "Sponsor");
        }
    }
}
