using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Otp.Core.Domains.Common.Enums;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    public partial class AddChannelPriceAndDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SubscriptionsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "ChannelPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Threshold = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChannelPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrincipalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discounts_ChannelPrices_ChannelPriceId",
                        column: x => x.ChannelPriceId,
                        principalTable: "ChannelPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Discounts_Principals_PrincipalId",
                        column: x => x.PrincipalId,
                        principalTable: "Principals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            
             // Seed SMS
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,.45.ToString(), "PH", "PH", Channel.Sms.ToString()});
            
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Threshold", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,10000.ToString(), .55.ToString(), "PH", "PH", Channel.Sms.ToString()});
            
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Threshold", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,5000.ToString(), .65.ToString(), "PH", "PH", Channel.Sms.ToString()});
            
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Threshold", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,1000.ToString(), .8.ToString(), "PH", "PH", Channel.Sms.ToString()});
            
            // Seed Email
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,.0075.ToString(), "PH", "PH", Channel.Email.ToString()});
            
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Threshold", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,100000.ToString(), .009.ToString(), "PH", "PH", Channel.Email.ToString()});
            
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Threshold", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,50000.ToString(), .02.ToString(), "PH", "PH", Channel.Email.ToString()});
            
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Threshold", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,10000.ToString(), .045.ToString(), "PH", "PH", Channel.Email.ToString()});
            
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Threshold", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,5000.ToString(), .06.ToString(), "PH", "PH", Channel.Email.ToString()});
            
            migrationBuilder.InsertData(
                table: "ChannelPrices",
                columns: new []{"Id", "CreatedAt", "Threshold", "Price", "Source", "Destination", "Channel"},
                values: new []{Guid.NewGuid().ToString(), DateTime.UtcNow.ToString() ,1000.ToString(), .075.ToString(), "PH", "PH", Channel.Email.ToString()});

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPrices_Id",
                table: "ChannelPrices",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ChannelPriceId",
                table: "Discounts",
                column: "ChannelPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_Id",
                table: "Discounts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_PrincipalId",
                table: "Discounts",
                column: "PrincipalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "ChannelPrices");

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrincipalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    TieredPlan = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Principals_PrincipalId",
                        column: x => x.PrincipalId,
                        principalTable: "Principals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SubscriptionsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Id",
                table: "Subscriptions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PrincipalId",
                table: "Subscriptions",
                column: "PrincipalId");
        }
    }
}
