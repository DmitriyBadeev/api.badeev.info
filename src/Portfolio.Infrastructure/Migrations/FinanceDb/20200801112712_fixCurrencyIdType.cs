using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.Infrastructure.Migrations.FinanceDb
{
    public partial class fixCurrencyIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "CurrencyOperations",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "CurrencyOperations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
