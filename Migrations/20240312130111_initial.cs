using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NewSky.API.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TebexId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceHt = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    PriceTtc = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    BanishmentEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserNumberVote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNumberVote", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoteReward",
                columns: table => new
                {
                    Position = table.Column<int>(type: "int", nullable: false),
                    Reward = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteReward", x => x.Position);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    IsEditable = table.Column<bool>(type: "bit", nullable: false),
                    HasPermission = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPackage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPackage_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPackage_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppSetting",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "DiscordUrl", "" },
                    { 2, "InstagramUrl", "" },
                    { 3, "YoutubeUrl", "" },
                    { 4, "TwitterUrl", "" },
                    { 5, "TikTokUrl", "" },
                    { 6, "WebStoreIdentifier", "q9p1-a1dc8b36df104edf3ec62d84d3851a78ef5dcf51" },
                    { 7, "XTebexSecret", "3421979d0b799fe1733ce64ad591b28d3bc86b77" },
                    { 8, "ServerIp", "" }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { -14, "Accès à la vue des rôles sur le panel admin", "access:admin-panel_roles" },
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
                table: "Role",
                columns: new[] { "Id", "Description", "IsDefault", "Name" },
                values: new object[,]
                {
                    { -3, "Développeur du site", true, "Développeur du site" },
                    { -2, "Responsable du serveur", true, "Fondateur" },
                    { -1, "Utilisateur authentifié", true, "Joueur" }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "HasPermission", "IsEditable", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { -113, true, false, -14, -3 },
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
                    { -63, true, false, -14, -2 },
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

            migrationBuilder.CreateIndex(
                name: "IX_AppSetting_Name",
                table: "AppSetting",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Package_TebexId",
                table: "Package",
                column: "TebexId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId_PermissionId",
                table: "RolePermission",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UUID",
                table: "User",
                column: "UUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPackage_PackageId",
                table: "UserPackage",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPackage_UserId_PackageId",
                table: "UserPackage",
                columns: new[] { "UserId", "PackageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId_UserId",
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSetting");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "UserNumberVote");

            migrationBuilder.DropTable(
                name: "UserPackage");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "VoteReward");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
