using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATMCompass.Insfrastructure.Data.Migrations
{
    public partial class Tables_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "Lon",
                table: "ATMs");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "ATMs",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ATMs",
                newName: "Fee");

            migrationBuilder.RenameColumn(
                name: "IsDriveThroughEnabled",
                table: "ATMs",
                newName: "WithinBank");

            migrationBuilder.RenameColumn(
                name: "IsAccessibleUsingWheelchair",
                table: "ATMs",
                newName: "Wheelchair");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                table: "ATMs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "ATMs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "ATMs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "ATMs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CashIn",
                table: "ATMs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Covered",
                table: "ATMs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "ATMs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DriveThrough",
                table: "ATMs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Indoor",
                table: "ATMs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NodeId",
                table: "ATMs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OperatorId",
                table: "ATMs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wikidata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wikipedia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BAM = table.Column<bool>(type: "bit", nullable: true),
                    EUR = table.Column<bool>(type: "bit", nullable: true),
                    USD = table.Column<bool>(type: "bit", nullable: true),
                    Others = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lon = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wikidata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wikipedia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_AddressId",
                table: "ATMs",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_BankId",
                table: "ATMs",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_BrandId",
                table: "ATMs",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_CurrencyId",
                table: "ATMs",
                column: "CurrencyId",
                unique: true,
                filter: "[CurrencyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_NodeId",
                table: "ATMs",
                column: "NodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ATMs_OperatorId",
                table: "ATMs",
                column: "OperatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ATMs_Addresses_AddressId",
                table: "ATMs",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ATMs_Banks_BankId",
                table: "ATMs",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ATMs_Brands_BrandId",
                table: "ATMs",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ATMs_Currencies_CurrencyId",
                table: "ATMs",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ATMs_Nodes_NodeId",
                table: "ATMs",
                column: "NodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ATMs_Operators_OperatorId",
                table: "ATMs",
                column: "OperatorId",
                principalTable: "Operators",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ATMs_Addresses_AddressId",
                table: "ATMs");

            migrationBuilder.DropForeignKey(
                name: "FK_ATMs_Banks_BankId",
                table: "ATMs");

            migrationBuilder.DropForeignKey(
                name: "FK_ATMs_Brands_BrandId",
                table: "ATMs");

            migrationBuilder.DropForeignKey(
                name: "FK_ATMs_Currencies_CurrencyId",
                table: "ATMs");

            migrationBuilder.DropForeignKey(
                name: "FK_ATMs_Nodes_NodeId",
                table: "ATMs");

            migrationBuilder.DropForeignKey(
                name: "FK_ATMs_Operators_OperatorId",
                table: "ATMs");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_AddressId",
                table: "ATMs");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_BankId",
                table: "ATMs");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_BrandId",
                table: "ATMs");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_CurrencyId",
                table: "ATMs");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_NodeId",
                table: "ATMs");

            migrationBuilder.DropIndex(
                name: "IX_ATMs_OperatorId",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Banks");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Banks");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "CashIn",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "Covered",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "DriveThrough",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "Indoor",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "NodeId",
                table: "ATMs");

            migrationBuilder.DropColumn(
                name: "OperatorId",
                table: "ATMs");

            migrationBuilder.RenameColumn(
                name: "WithinBank",
                table: "ATMs",
                newName: "IsDriveThroughEnabled");

            migrationBuilder.RenameColumn(
                name: "Wheelchair",
                table: "ATMs",
                newName: "IsAccessibleUsingWheelchair");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "ATMs",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "Fee",
                table: "ATMs",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                table: "ATMs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ATMs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "ATMs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ATMs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lon",
                table: "ATMs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });
        }
    }
}
