using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupermarketApp.Data.Migrations
{
    public partial class notrequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "CartItems",
                newName: "Quantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "CartItems",
                newName: "quantity");
        }
    }
}
