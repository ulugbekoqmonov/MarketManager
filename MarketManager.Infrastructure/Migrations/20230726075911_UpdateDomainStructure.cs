using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDomainStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemPriceSum",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ItemPurchaseSum",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "SoldPrice",
                table: "Items",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "Items",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Clients",
                newName: "CashbackSum");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MeasureType",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPriceBeforeCashback",
                table: "Orders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Clients",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "TotalPriceBeforeCashback",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Items",
                newName: "SoldPrice");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Items",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "CashbackSum",
                table: "Clients",
                newName: "TotalPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "ItemPriceSum",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemPurchaseSum",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Clients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
