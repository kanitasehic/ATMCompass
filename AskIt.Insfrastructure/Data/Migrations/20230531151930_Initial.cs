using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATMCompass.Insfrastructure.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ATMs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lon = table.Column<double>(type: "float", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wheelchair = table.Column<bool>(type: "bit", nullable: true),
                    DriveThrough = table.Column<bool>(type: "bit", nullable: true),
                    CashIn = table.Column<bool>(type: "bit", nullable: true),
                    Indoor = table.Column<bool>(type: "bit", nullable: true),
                    Covered = table.Column<bool>(type: "bit", nullable: true),
                    WithinBank = table.Column<bool>(type: "bit", nullable: true),
                    OpeningHours = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATMs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ATMs");
        }
    }
}
