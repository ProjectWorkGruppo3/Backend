using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serendipity.Infrastructure.Migrations
{
    public partial class RenameGlobalStatsDateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Giorno",
                table: "GlobalStatistics",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_GlobalStatistics_Giorno",
                table: "GlobalStatistics",
                newName: "IX_GlobalStatistics_Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "GlobalStatistics",
                newName: "Giorno");

            migrationBuilder.RenameIndex(
                name: "IX_GlobalStatistics_Date",
                table: "GlobalStatistics",
                newName: "IX_GlobalStatistics_Giorno");
        }
    }
}
