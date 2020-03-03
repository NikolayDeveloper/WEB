using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class UpdateDicTariffsDeletePrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceBeneficiary",
                table: "DicTariffs");

            migrationBuilder.DropColumn(
                name: "PriceBusiness",
                table: "DicTariffs");

            migrationBuilder.DropColumn(
                name: "PriceFl",
                table: "DicTariffs");

            migrationBuilder.DropColumn(
                name: "PriceUl",
                table: "DicTariffs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceBeneficiary",
                table: "DicTariffs",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceBusiness",
                table: "DicTariffs",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceFl",
                table: "DicTariffs",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceUl",
                table: "DicTariffs",
                nullable: true);
        }
    }
}
