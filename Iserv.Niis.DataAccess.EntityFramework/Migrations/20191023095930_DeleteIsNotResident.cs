using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DeleteIsNotResident : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql
            (
                @"IF EXISTS 
                    (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DicCustomers' AND COLUMN_NAME = 'IsNotResident')
                  BEGIN
                    ALTER TABLE DicCustomers DROP COLUMN IsNotResident
                  END"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
