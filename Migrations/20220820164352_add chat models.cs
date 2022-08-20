﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class addchatmodels : Migration
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
                name: "categories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    cateName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "chatRooms",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    isGroupChat = table.Column<bool>(type: "INTEGER", nullable: false),
                    roomName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatRooms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    departmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    departmentName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.departmentId);
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    userId = table.Column<Guid>(type: "TEXT", nullable: false),
                    userCode = table.Column<string>(type: "TEXT", nullable: true),
                    firstName = table.Column<string>(type: "TEXT", nullable: true),
                    lastName = table.Column<string>(type: "TEXT", nullable: true),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    role = table.Column<string>(type: "TEXT", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    TokenCreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TokenExpireDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    departmentId = table.Column<Guid>(type: "TEXT", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_AspNetUsers_departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "departments",
                        principalColumn: "departmentId");
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
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
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
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "blocks",
                columns: table => new
                {
                    blockId = table.Column<Guid>(type: "TEXT", nullable: false),
                    blockName = table.Column<string>(type: "TEXT", nullable: false),
                    flourAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    blockManagerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocks", x => x.blockId);
                    table.ForeignKey(
                        name: "FK_blocks_AspNetUsers_blockManagerId",
                        column: x => x.blockManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "chatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    message = table.Column<string>(type: "TEXT", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    userId = table.Column<string>(type: "TEXT", nullable: true),
                    RoomID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chatMessages_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_chatMessages_chatRooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "chatRooms",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "issues",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    status = table.Column<string>(type: "TEXT", nullable: true),
                    feedback = table.Column<string>(type: "TEXT", nullable: true),
                    createdDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    lastModifiedDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    isPrivate = table.Column<bool>(type: "INTEGER", nullable: false),
                    cateId = table.Column<Guid>(type: "TEXT", nullable: false),
                    categoryID = table.Column<Guid>(type: "TEXT", nullable: true),
                    authorId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_issues_AspNetUsers_authorId",
                        column: x => x.authorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_issues_categories_categoryID",
                        column: x => x.categoryID,
                        principalTable: "categories",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "userConnections",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomID = table.Column<Guid>(type: "TEXT", nullable: true),
                    userId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userConnections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_userConnections_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_userConnections_chatRooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "chatRooms",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "apartments",
                columns: table => new
                {
                    apartmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    apartmentName = table.Column<string>(type: "TEXT", nullable: true),
                    heartWallArea = table.Column<int>(type: "INTEGER", nullable: false),
                    clearanceArea = table.Column<int>(type: "INTEGER", nullable: false),
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
                name: "assignIssues",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    staffId = table.Column<string>(type: "TEXT", nullable: true),
                    issueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    isResolved = table.Column<bool>(type: "INTEGER", nullable: false),
                    isConfirmedByAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    isConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    staffFeedback = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignIssues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_assignIssues_AspNetUsers_staffId",
                        column: x => x.staffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_assignIssues_issues_issueId",
                        column: x => x.issueId,
                        principalTable: "issues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    isPrivate = table.Column<bool>(type: "INTEGER", nullable: false),
                    isChild = table.Column<bool>(type: "INTEGER", nullable: false),
                    parentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    authorId = table.Column<string>(type: "TEXT", nullable: true),
                    authorsId = table.Column<string>(type: "TEXT", nullable: true),
                    issueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    issuesID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_comments_AspNetUsers_authorsId",
                        column: x => x.authorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_comments_issues_issuesID",
                        column: x => x.issuesID,
                        principalTable: "issues",
                        principalColumn: "ID");
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

            migrationBuilder.CreateTable(
                name: "filePaths",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    filePath = table.Column<string>(type: "TEXT", nullable: true),
                    issueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    issuesID = table.Column<Guid>(type: "TEXT", nullable: true),
                    userId = table.Column<string>(type: "TEXT", nullable: true),
                    usersId = table.Column<string>(type: "TEXT", nullable: true),
                    assignIssueId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filePaths", x => x.ID);
                    table.ForeignKey(
                        name: "FK_filePaths_AspNetUsers_usersId",
                        column: x => x.usersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_filePaths_assignIssues_assignIssueId",
                        column: x => x.assignIssueId,
                        principalTable: "assignIssues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_filePaths_issues_issuesID",
                        column: x => x.issuesID,
                        principalTable: "issues",
                        principalColumn: "ID");
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
                name: "IX_AspNetUsers_departmentId",
                table: "AspNetUsers",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assignIssues_issueId",
                table: "assignIssues",
                column: "issueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assignIssues_staffId",
                table: "assignIssues",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_blocks_blockManagerId",
                table: "blocks",
                column: "blockManagerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chatMessages_RoomID",
                table: "chatMessages",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_chatMessages_userId",
                table: "chatMessages",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_authorsId",
                table: "comments",
                column: "authorsId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_issuesID",
                table: "comments",
                column: "issuesID");

            migrationBuilder.CreateIndex(
                name: "IX_filePaths_assignIssueId",
                table: "filePaths",
                column: "assignIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_filePaths_issuesID",
                table: "filePaths",
                column: "issuesID");

            migrationBuilder.CreateIndex(
                name: "IX_filePaths_usersId",
                table: "filePaths",
                column: "usersId");

            migrationBuilder.CreateIndex(
                name: "IX_issues_authorId",
                table: "issues",
                column: "authorId");

            migrationBuilder.CreateIndex(
                name: "IX_issues_categoryID",
                table: "issues",
                column: "categoryID");

            migrationBuilder.CreateIndex(
                name: "IX_rentalHistories_apartmentId",
                table: "rentalHistories",
                column: "apartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_rentalHistories_applicationUserId",
                table: "rentalHistories",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_userConnections_RoomID",
                table: "userConnections",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_userConnections_userId",
                table: "userConnections",
                column: "userId");

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
                name: "chatMessages");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "filePaths");

            migrationBuilder.DropTable(
                name: "rentalHistories");

            migrationBuilder.DropTable(
                name: "socialRecognizations");

            migrationBuilder.DropTable(
                name: "userConnections");

            migrationBuilder.DropTable(
                name: "userHistories");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "assignIssues");

            migrationBuilder.DropTable(
                name: "chatRooms");

            migrationBuilder.DropTable(
                name: "apartments");

            migrationBuilder.DropTable(
                name: "issues");

            migrationBuilder.DropTable(
                name: "blocks");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}