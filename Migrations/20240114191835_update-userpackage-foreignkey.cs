using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewSky.API.Migrations
{
    /// <inheritdoc />
    public partial class updateuserpackageforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPackage_User_PackageId",
                table: "UserPackage");

            migrationBuilder.CreateIndex(
                name: "IX_UserPackage_UserId",
                table: "UserPackage",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPackage_User_UserId",
                table: "UserPackage",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPackage_User_UserId",
                table: "UserPackage");

            migrationBuilder.DropIndex(
                name: "IX_UserPackage_UserId",
                table: "UserPackage");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPackage_User_PackageId",
                table: "UserPackage",
                column: "PackageId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
