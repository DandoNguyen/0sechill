using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class initialmigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    userId = table.Column<Guid>(type: "TEXT", nullable: false),
                    userCode = table.Column<string>(type: "TEXT", nullable: true),
                    firstName = table.Column<string>(type: "TEXT", nullable: true),
                    lastName = table.Column<string>(type: "TEXT", nullable: true),
                    DOB = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    role = table.Column<string>(type: "TEXT", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    TokenCreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TokenExpireDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blocks",
                columns: table => new
                {
                    blockId = table.Column<Guid>(type: "TEXT", nullable: false),
                    blockName = table.Column<string>(type: "TEXT", nullable: false),
                    flourAmount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocks", x => x.blockId);
                });

            migrationBuilder.CreateTable(
                name: "socialRecognizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    number = table.Column<string>(type: "TEXT", nullable: true),
                    type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socialRecognizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apartments",
                columns: table => new
                {
                    apartmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    heartWallArea = table.Column<int>(type: "INTEGER", nullable: false),
                    clearanceArea = table.Column<int>(type: "INTEGER", nullable: false),
                    ísFurnitureAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    bedroomAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    blockId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apartments", x => x.apartmentId);
                    table.ForeignKey(
                        name: "FK_apartments_blocks_blockId",
                        column: x => x.blockId,
                        principalTable: "blocks",
                        principalColumn: "blockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rentalHistories",
                columns: table => new
                {
                    rentalHistoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    startDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    endDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    lastSignedDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    createdDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    modifiedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    applicationUserId = table.Column<string>(type: "TEXT", nullable: true),
                    userId = table.Column<Guid>(type: "TEXT", nullable: false),
                    apartmentId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentalHistories", x => x.rentalHistoryId);
                    table.ForeignKey(
                        name: "FK_rentalHistories_apartments_apartmentId",
                        column: x => x.apartmentId,
                        principalTable: "apartments",
                        principalColumn: "apartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rentalHistories_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "userHistories",
                columns: table => new
                {
                    userHistoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    startDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    endDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    lastSignedDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    createdDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    modifiedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    applicationUserId = table.Column<string>(type: "TEXT", nullable: true),
                    userId = table.Column<Guid>(type: "TEXT", nullable: false),
                    apartmentId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userHistories", x => x.userHistoryId);
                    table.ForeignKey(
                        name: "FK_userHistories_apartments_apartmentId",
                        column: x => x.apartmentId,
                        principalTable: "apartments",
                        principalColumn: "apartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userHistories_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_apartments_blockId",
                table: "apartments",
                column: "blockId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rentalHistories_apartmentId",
                table: "rentalHistories",
                column: "apartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_rentalHistories_applicationUserId",
                table: "rentalHistories",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_userHistories_apartmentId",
                table: "userHistories",
                column: "apartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_userHistories_applicationUserId",
                table: "userHistories",
                column: "applicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "rentalHistories");

            migrationBuilder.DropTable(
                name: "socialRecognizations");

            migrationBuilder.DropTable(
                name: "userHistories");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "apartments");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "blocks");
        }
    }
}
