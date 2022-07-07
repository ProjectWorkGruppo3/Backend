using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serendipity.Infrastructure.Migrations
{
    public partial class AddKeyToAlarms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_Alarms",
                table: "Alarms",
                columns: new[] { "Timestamp", "DeviceId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Alarms",
                table: "Alarms");
        }
    }
}
