using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpiredProduct_DeletedTime_deleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "ExpiredProducts");

            migrationBuilder.AddColumn<double>(
                name: "ExistCount",
                table: "Packages",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExistCount",
                table: "Packages");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "ExpiredProducts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
