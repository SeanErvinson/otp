using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    public partial class AddedAppIdToCallbackEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppId",
                table: "CallbackEvents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CallbackEvents_AppId",
                table: "CallbackEvents",
                column: "AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_CallbackEvents_Apps_AppId",
                table: "CallbackEvents",
                column: "AppId",
                principalTable: "Apps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CallbackEvents_Apps_AppId",
                table: "CallbackEvents");

            migrationBuilder.DropIndex(
                name: "IX_CallbackEvents_AppId",
                table: "CallbackEvents");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "CallbackEvents");
        }
    }
}
