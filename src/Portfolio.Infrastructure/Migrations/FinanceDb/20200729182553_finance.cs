using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.Infrastructure.Migrations.FinanceDb
{
    public partial class finance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetOperations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ticket = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    PaymentPrice = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PortfolioId = table.Column<int>(nullable: false),
                    AssetTypeId = table.Column<int>(nullable: false),
                    AssetActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetOperations_AssetActions_AssetActionId",
                        column: x => x.AssetActionId,
                        principalTable: "AssetActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetOperations_AssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "AssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetOperations_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyOperations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrencyName = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CurrencyActionId = table.Column<int>(nullable: false),
                    PortfolioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyOperations_CurrencyActions_CurrencyActionId",
                        column: x => x.CurrencyActionId,
                        principalTable: "CurrencyActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrencyOperations_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetOperations_AssetActionId",
                table: "AssetOperations",
                column: "AssetActionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetOperations_AssetTypeId",
                table: "AssetOperations",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetOperations_PortfolioId",
                table: "AssetOperations",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyOperations_CurrencyActionId",
                table: "CurrencyOperations",
                column: "CurrencyActionId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyOperations_PortfolioId",
                table: "CurrencyOperations",
                column: "PortfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetOperations");

            migrationBuilder.DropTable(
                name: "CurrencyOperations");

            migrationBuilder.DropTable(
                name: "AssetActions");

            migrationBuilder.DropTable(
                name: "AssetTypes");

            migrationBuilder.DropTable(
                name: "CurrencyActions");

            migrationBuilder.DropTable(
                name: "Portfolios");
        }
    }
}
