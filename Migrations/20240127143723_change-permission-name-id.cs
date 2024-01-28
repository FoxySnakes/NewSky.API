using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NewSky.API.Migrations
{
    /// <inheritdoc />
    public partial class changepermissionnameid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -31);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -30);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -29);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -28);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -27);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -26);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -25);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -24);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -23);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -22);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -21);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -20);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -19);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -18);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -17);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -16);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -15);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -14);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { -13, "Supprimer un rôle", "delete:role" },
                    { -12, "Modifier un rôle", "update:role" },
                    { -11, "Modifier le rôle d'un utilisateur", "update:user_role" },
                    { -10, "Modifier les paramètres généraux du site", "update:general-settings" },
                    { -9, "Modifier la sanction de l'utilisateur", "update:user_punishment" },
                    { -8, "Modifier les rôles et nom d'utilisateur d'un utilisateur", "update:user_informations" },
                    { -7, "Créer un Rôle", "create:role" },
                    { -6, "Accès à la vue des paramètres généraux", "access:admin-panel_general-settings" },
                    { -5, "Accès à la vue des votes sur le panel admin", "access:admin-panel_votes" },
                    { -4, "Accès à la vue des utilisateurs sur le panel admin", "access:admin-panel_users" },
                    { -3, "Accès à la vue des ventes sur le panel admin", "access:admin-panel_sales" },
                    { -2, "Accès à la vue du dashboard sur le panel admin", "access:admin-panel_dashboard" },
                    { -1, "Accès au panel admin du site", "access:admin-panel" }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "HasPermission", "IsEditable", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { -112, true, false, -13, -3 },
                    { -111, true, false, -12, -3 },
                    { -110, true, false, -11, -3 },
                    { -109, true, false, -10, -3 },
                    { -108, true, false, -9, -3 },
                    { -107, true, false, -8, -3 },
                    { -106, true, false, -7, -3 },
                    { -105, true, false, -6, -3 },
                    { -104, true, false, -5, -3 },
                    { -103, true, false, -4, -3 },
                    { -102, true, false, -3, -3 },
                    { -101, true, false, -2, -3 },
                    { -100, true, false, -1, -3 },
                    { -62, true, false, -13, -2 },
                    { -61, true, false, -12, -2 },
                    { -60, true, false, -11, -2 },
                    { -59, true, false, -10, -2 },
                    { -58, true, false, -9, -2 },
                    { -57, true, false, -8, -2 },
                    { -56, true, false, -7, -2 },
                    { -55, true, false, -6, -2 },
                    { -54, true, false, -5, -2 },
                    { -53, true, false, -4, -2 },
                    { -52, true, false, -3, -2 },
                    { -51, true, false, -2, -2 },
                    { -50, true, false, -1, -2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -112);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -111);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -110);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -109);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -108);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -107);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -106);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -105);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -104);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -103);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -102);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -101);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -100);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -62);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -61);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -60);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -59);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -58);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -57);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -56);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -55);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -54);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -53);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -52);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -51);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: -50);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Accès au panel admin du site", "access:admin-panel" },
                    { 2, "Accès à la vue du dashboard sur le panel admin", "access:admin-panel_dashboard" },
                    { 3, "Accès à la vue des ventes sur le panel admin", "access:admin-panel_sales" },
                    { 4, "Accès à la vue des utilisateurs sur le panel admin", "access:admin-panel_users" },
                    { 5, "Accès à la vue des votes sur le panel admin", "access:admin-panel_votes" },
                    { 6, "Accès à la vue des paramètres généraux", "access:admin-panel_general-settings" },
                    { 7, "Créer un Rôle", "create:role" },
                    { 8, "Modifier les permissions d'un utilisateur", "update:user_permissions" },
                    { 9, "Modifier le pseudo d'un utilisateur", "update:user_username" },
                    { 10, "Gérer l'état d'un compte utilisateur", "update:user_status" },
                    { 11, "Gérer le panier de l'utilisateur authentifié", "update:user_cart" },
                    { 12, "Modifier les paramètres généraux du site", "update:general-settings" },
                    { 13, "Modifier le rôle d'un utilisateur", "update:user_role" },
                    { 14, "Modifier un rôle", "update:role" },
                    { 15, "Supprimer un rôle", "delete:role" }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "HasPermission", "IsEditable", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { -31, true, false, 15, -3 },
                    { -30, true, false, 14, -3 },
                    { -29, true, false, 13, -3 },
                    { -28, true, false, 12, -3 },
                    { -27, true, false, 11, -3 },
                    { -26, true, false, 10, -3 },
                    { -25, true, false, 9, -3 },
                    { -24, true, false, 8, -3 },
                    { -23, true, false, 7, -3 },
                    { -22, true, false, 6, -3 },
                    { -21, true, false, 5, -3 },
                    { -20, true, false, 4, -3 },
                    { -19, true, false, 3, -3 },
                    { -18, true, false, 2, -3 },
                    { -17, true, false, 1, -3 },
                    { -16, true, false, 15, -2 },
                    { -15, true, false, 14, -2 },
                    { -14, true, false, 13, -2 },
                    { -13, true, false, 12, -2 },
                    { -12, true, false, 11, -2 },
                    { -11, true, false, 10, -2 },
                    { -10, true, false, 9, -2 },
                    { -9, true, false, 8, -2 },
                    { -8, true, false, 7, -2 },
                    { -7, true, false, 6, -2 },
                    { -6, true, false, 5, -2 },
                    { -5, true, false, 4, -2 },
                    { -4, true, false, 3, -2 },
                    { -3, true, false, 2, -2 },
                    { -2, true, false, 1, -2 },
                    { -1, true, false, 11, -1 }
                });
        }
    }
}
