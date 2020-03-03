using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class UpdateRequestFlagIsSynkRegNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSyncRequestNum",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSyncRequestNum",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
