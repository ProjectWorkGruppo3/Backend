using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serendipity.Infrastructure.Migrations
{
    public partial class AddGlobalStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalStatistics",
                columns: table => new
                {
                    Giorno = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataIngested = table.Column<long>(type: "bigint", nullable: false),
                    Falls = table.Column<long>(type: "bigint", nullable: false),
                    Serendipity = table.Column<int>(type: "integer", nullable: false),
                    LocationDensity = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalStatistics", x => x.Giorno);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalStatistics_Giorno",
                table: "GlobalStatistics",
                column: "Giorno");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalStatistics");
        }
    }
}
