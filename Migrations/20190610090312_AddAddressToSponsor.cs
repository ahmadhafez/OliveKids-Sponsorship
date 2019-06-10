using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OliveKids.Migrations
{
    public partial class AddAddressToSponsor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Sponsor");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Sponsor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Sponsor");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Sponsor",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
