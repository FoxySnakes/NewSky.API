using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewSky.API.Migrations
{
    /// <inheritdoc />
    public partial class addtokeninvalid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenInvalid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenInvalid", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenInvalid_Value",
                table: "TokenInvalid",
                column: "Value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenInvalid");
        }
    }
}
