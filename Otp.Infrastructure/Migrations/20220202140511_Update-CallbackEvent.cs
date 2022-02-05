using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    public partial class UpdateCallbackEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CallbackEvent",
                table: "CallbackEvent");

            migrationBuilder.RenameTable(
                name: "CallbackEvent",
                newName: "CallbackEvents");

            migrationBuilder.RenameIndex(
                name: "IX_CallbackEvent_Id",
                table: "CallbackEvents",
                newName: "IX_CallbackEvents_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CallbackEvents",
                table: "CallbackEvents",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CallbackEvents",
                table: "CallbackEvents");

            migrationBuilder.RenameTable(
                name: "CallbackEvents",
                newName: "CallbackEvent");

            migrationBuilder.RenameIndex(
                name: "IX_CallbackEvents_Id",
                table: "CallbackEvent",
                newName: "IX_CallbackEvent_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CallbackEvent",
                table: "CallbackEvent",
                column: "Id");
        }
    }
}
