using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    public partial class AddedOtpAttempts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Retries",
                table: "OtpRequests",
                newName: "ResendCount");

            migrationBuilder.AddColumn<int>(
                name: "MaxAttempts",
                table: "OtpRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "CallbackEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OtpAttempt",
                columns: table => new
                {
                    OtpRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttemptedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttemptStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtpAttempt");

            migrationBuilder.DropColumn(
                name: "MaxAttempts",
                table: "OtpRequests");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "CallbackEvents");

            migrationBuilder.RenameColumn(
                name: "ResendCount",
                table: "OtpRequests",
                newName: "Retries");
        }
    }
}
