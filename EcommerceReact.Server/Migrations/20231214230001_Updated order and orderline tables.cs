using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceReact.Server.Migrations
{
    /// <inheritdoc />
    public partial class Updatedorderandorderlinetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Orders_OrderNumber",
                table: "OrderLines");

            migrationBuilder.AlterColumn<int>(
                name: "OrderNumber",
                table: "OrderLines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Orders_OrderNumber",
                table: "OrderLines",
                column: "OrderNumber",
                principalTable: "Orders",
                principalColumn: "OrderNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Orders_OrderNumber",
                table: "OrderLines");

            migrationBuilder.AlterColumn<int>(
                name: "OrderNumber",
                table: "OrderLines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Orders_OrderNumber",
                table: "OrderLines",
                column: "OrderNumber",
                principalTable: "Orders",
                principalColumn: "OrderNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
