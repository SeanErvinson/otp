using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    public partial class AddedTagsConversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Apps",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Apps");
        }
    }
}
