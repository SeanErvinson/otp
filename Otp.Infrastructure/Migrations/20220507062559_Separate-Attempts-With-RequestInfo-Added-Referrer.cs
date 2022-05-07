using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    public partial class SeparateAttemptsWithRequestInfoAddedReferrer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtpAttempt");

            migrationBuilder.AddColumn<string>(
                name: "RequestInfo_IpAddress",
                table: "OtpRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestInfo_Referrer",
                table: "OtpRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestInfo_UserAgent",
                table: "OtpRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OtpAttempts",
                columns: table => new
                {
                    OtpRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttemptedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttemptStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpAttempts", x => new { x.OtpRequestId, x.Id });
                    table.ForeignKey(
                        name: "FK_OtpAttempts_OtpRequests_OtpRequestId",
                        column: x => x.OtpRequestId,
                        principalTable: "OtpRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtpAttempts");

            migrationBuilder.DropColumn(
                name: "RequestInfo_IpAddress",
                table: "OtpRequests");

            migrationBuilder.DropColumn(
                name: "RequestInfo_Referrer",
                table: "OtpRequests");

            migrationBuilder.DropColumn(
                name: "RequestInfo_UserAgent",
                table: "OtpRequests");

            migrationBuilder.CreateTable(
                name: "OtpAttempt",
                columns: table => new
                {
                    OtpRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttemptStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttemptedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpAttempt", x => new { x.OtpRequestId, x.Id });
                    table.ForeignKey(
                        name: "FK_OtpAttempt_OtpRequests_OtpRequestId",
                        column: x => x.OtpRequestId,
                        principalTable: "OtpRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
