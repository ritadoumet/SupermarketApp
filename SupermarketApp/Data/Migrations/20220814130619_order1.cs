using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupermarketApp.Data.Migrations
{
    public partial class order1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrderID",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "CartItems",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_OrderID",
                table: "CartItems",
                newName: "IX_CartItems_OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "CartItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "CartItems",
                newName: "OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems",
                newName: "IX_CartItems_OrderID");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrderID",
                table: "CartItems",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
