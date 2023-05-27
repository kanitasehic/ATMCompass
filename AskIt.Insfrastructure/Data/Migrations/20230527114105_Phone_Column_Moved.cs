using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATMCompass.Insfrastructure.Data.Migrations
{
    public partial class Phone_Column_Moved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "ATMs");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Banks");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "ATMs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
