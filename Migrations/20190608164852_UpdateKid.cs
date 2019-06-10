using Microsoft.EntityFrameworkCore.Migrations;

namespace OliveKids.Migrations
{
    public partial class UpdateKid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Kid",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Kid",
                nullable: true);
        }
        /*
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Kid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Kid",
                nullable: true,
                oldClrType: typeof(string));
        }

    */
    }
}
