using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NewSky.API.Migrations
{
    /// <inheritdoc />
    public partial class addpermissionroleaccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { -14, "Accès à la vue des rôles sur le panel admin", "access:admin-panel_roles" });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "HasPermission", "IsEditable", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { -113, true, false, -14, -3 },
                    { -63, true, false, -14, -2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -113);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -63);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -14);
        }
    }
}
