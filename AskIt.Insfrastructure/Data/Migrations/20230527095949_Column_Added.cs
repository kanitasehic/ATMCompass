using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATMCompass.Insfrastructure.Data.Migrations
{
    public partial class Column_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApprovedByAdmin",
                table: "ATMs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedByAdmin",
                table: "ATMs");
        }
    }
}
