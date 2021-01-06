using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.Infrastructure.Migrations.FinanceDb
{
    public partial class AddLinkToPortfolioForPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PortfolioId",
                table: "Payments",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Portfolios_PortfolioId",
                table: "Payments",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Portfolios_PortfolioId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PortfolioId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Payments");
        }
    }
}
