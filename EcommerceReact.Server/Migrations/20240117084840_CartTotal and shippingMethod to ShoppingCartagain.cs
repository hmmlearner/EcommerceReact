using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceReact.Server.Migrations
{
    /// <inheritdoc />
    public partial class CartTotalandshippingMethodtoShoppingCartagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CartTotal",
                table: "ShoppingCarts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ShippingMethod",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartTotal",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShippingMethod",
                table: "ShoppingCarts");
        }
    }
}
