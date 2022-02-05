using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    public partial class RenameModeToChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mode",
                table: "OtpRequests",
                newName: "Channel");

            migrationBuilder.RenameColumn(
                name: "Mode",
                table: "CallbackEvents",
                newName: "Channel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Channel",
                table: "OtpRequests",
                newName: "Mode");

            migrationBuilder.RenameColumn(
                name: "Channel",
                table: "CallbackEvents",
                newName: "Mode");
        }
    }
}
