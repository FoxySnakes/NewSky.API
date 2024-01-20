using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewSky.API.Migrations
{
    /// <inheritdoc />
    public partial class addpriceht : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Package",
                newName: "PriceTtc");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceHt",
                table: "Package",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceHt",
                table: "Package");

            migrationBuilder.RenameColumn(
                name: "PriceTtc",
                table: "Package",
                newName: "TotalPrice");
        }
    }
}
