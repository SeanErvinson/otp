using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    public partial class RenamedSecretToKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Subscriptions_SubscriptionId",
                table: "Discount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount",
                table: "Discount");

            migrationBuilder.RenameTable(
                name: "Discount",
                newName: "Discounts");

            migrationBuilder.RenameColumn(
                name: "Secret",
                table: "OtpRequests",
                newName: "Key");

            migrationBuilder.RenameIndex(
                name: "IX_Discount_SubscriptionId",
                table: "Discounts",
                newName: "IX_Discounts_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Discount_Id",
                table: "Discounts",
                newName: "IX_Discounts_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Subscriptions_SubscriptionId",
                table: "Discounts",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Subscriptions_SubscriptionId",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts");

            migrationBuilder.RenameTable(
                name: "Discounts",
                newName: "Discount");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "OtpRequests",
                newName: "Secret");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_SubscriptionId",
                table: "Discount",
                newName: "IX_Discount_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_Id",
                table: "Discount",
                newName: "IX_Discount_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount",
                table: "Discount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Subscriptions_SubscriptionId",
                table: "Discount",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
