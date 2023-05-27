using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATMCompass.Insfrastructure.Data.Migrations
{
    public partial class Constraints_Changed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ATMs_BankId",
                table: "ATMs");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_BrandId",
                table: "ATMs");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_OperatorId",
                table: "ATMs");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_BankId",
                table: "ATMs",
                column: "BankId",
                unique: true,
                filter: "[BankId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_BrandId",
                table: "ATMs",
                column: "BrandId",
                unique: true,
                filter: "[BrandId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_OperatorId",
                table: "ATMs",
                column: "OperatorId",
                unique: true,
                filter: "[OperatorId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ATMs_BankId",
                table: "ATMs");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_BrandId",
                table: "ATMs");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_OperatorId",
                table: "ATMs");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_BankId",
                table: "ATMs",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_BrandId",
                table: "ATMs",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_OperatorId",
                table: "ATMs",
                column: "OperatorId");
        }
    }
}
