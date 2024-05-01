using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceReact.Server.Migrations
{
    /// <inheritdoc />
    public partial class OrdersandOrders_shoppingcart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Orders_ShoppingCart_Order_ShoppingCartId",
                table: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "Orders_ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_Order_ShoppingCartId",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Order_ShoppingCartId",
                table: "ShoppingCartItems");

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLines_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "OrderNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_OrderNumber",
                table: "OrderLines",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_ProductId",
                table: "OrderLines",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.AddColumn<int>(
                name: "Order_ShoppingCartId",
                table: "ShoppingCartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders_ShoppingCart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders_ShoppingCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_ShoppingCart_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "OrderNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_Order_ShoppingCartId",
                table: "ShoppingCartItems",
                column: "Order_ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShoppingCart_OrderNumber",
                table: "Orders_ShoppingCart",
                column: "OrderNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Orders_ShoppingCart_Order_ShoppingCartId",
                table: "ShoppingCartItems",
                column: "Order_ShoppingCartId",
                principalTable: "Orders_ShoppingCart",
                principalColumn: "Id");
        }
    }
}
