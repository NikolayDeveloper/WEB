using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class SetExpirationIntoStages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update DicRouteStages set ExpirationValue = 3, ExpirationType = 1 where Code = 'IN2.2'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
