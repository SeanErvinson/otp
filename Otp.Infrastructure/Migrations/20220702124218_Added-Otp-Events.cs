using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    public partial class AddedOtpEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OtpRequests");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "OtpRequests");

            migrationBuilder.RenameColumn(
                name: "RequestInfo_UserAgent",
                table: "OtpRequests",
                newName: "ClientInfo_UserAgent");

            migrationBuilder.RenameColumn(
                name: "RequestInfo_Referrer",
                table: "OtpRequests",
                newName: "ClientInfo_Referrer");

            migrationBuilder.RenameColumn(
                name: "RequestInfo_IpAddress",
                table: "OtpRequests",
                newName: "ClientInfo_IpAddress");

            migrationBuilder.RenameColumn(
                name: "Contact",
                table: "OtpRequests",
                newName: "Recipient");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "OtpRequests",
                newName: "Availability");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "OtpRequests",
                newName: "AuthenticityKey");

            migrationBuilder.CreateTable(
                name: "Timeline",
                columns: table => new
                {
                    OtpRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccuredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timeline", x => new { x.OtpRequestId, x.Id });
                    table.ForeignKey(
                        name: "FK_Timeline_OtpRequests_OtpRequestId",
                        column: x => x.OtpRequestId,
                        principalTable: "OtpRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtpRequests_CreatedAt_Id",
                table: "OtpRequests",
                columns: new[] { "CreatedAt", "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timeline");

            migrationBuilder.DropIndex(
                name: "IX_OtpRequests_CreatedAt_Id",
                table: "OtpRequests");

            migrationBuilder.RenameColumn(
                name: "ClientInfo_UserAgent",
                table: "OtpRequests",
                newName: "RequestInfo_UserAgent");

            migrationBuilder.RenameColumn(
                name: "ClientInfo_Referrer",
                table: "OtpRequests",
                newName: "RequestInfo_Referrer");

            migrationBuilder.RenameColumn(
                name: "ClientInfo_IpAddress",
                table: "OtpRequests",
                newName: "RequestInfo_IpAddress");

            migrationBuilder.RenameColumn(
                name: "Recipient",
                table: "OtpRequests",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Availability",
                table: "OtpRequests",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "AuthenticityKey",
                table: "OtpRequests",
                newName: "Key");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "OtpRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "OtpRequests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
