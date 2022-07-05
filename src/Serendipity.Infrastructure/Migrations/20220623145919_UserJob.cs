using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serendipity.Infrastructure.Migrations
{
    public partial class UserJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Job",
                table: "AspNetUsers");
        }
    }
}
