using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class init : Migration
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
                    roomName = table.Column<string>(type: "TEXT", nullable: true),
                    groupAdmin = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "notifications",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    receiverId = table.Column<string>(type: "TEXT", nullable: true),
                    isSeen = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.ID);
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
                    userCode = table.Column<string>(type: "TEXT", nullable: true),
                    firstName = table.Column<string>(type: "TEXT", nullable: true),
                    lastName = table.Column<string>(type: "TEXT", nullable: true),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    isActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    age = table.Column<int>(type: "INTEGER", nullable: false),
                    isMale = table.Column<bool>(type: "INTEGER", nullable: false),
                    IDType = table.Column<string>(type: "TEXT", nullable: true),
                    IDNumber = table.Column<string>(type: "TEXT", nullable: true),
                    roleID = table.Column<string>(type: "TEXT", nullable: true),
                    residentialAddress = table.Column<string>(type: "TEXT", nullable: true),
                    phoneCountryCode = table.Column<string>(type: "TEXT", nullable: true),
                    role = table.Column<string>(type: "TEXT", nullable: true),
                    currentHubConnectionId = table.Column<string>(type: "TEXT", nullable: true),
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
                name: "ApplicationUserRoom",
                columns: table => new
                {
                    chatRoomsID = table.Column<Guid>(type: "TEXT", nullable: false),
                    usersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRoom", x => new { x.chatRoomsID, x.usersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRoom_AspNetUsers_usersId",
                        column: x => x.usersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRoom_chatRooms_chatRoomsID",
                        column: x => x.chatRoomsID,
                        principalTable: "chatRooms",
                        principalColumn: "ID",
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
                name: "bookingTasks",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateOfBooking = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    TimeLevelOfBooking = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    isAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    userID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookingTasks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_bookingTasks_AspNetUsers_userID",
                        column: x => x.userID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "chatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    message = table.Column<string>(type: "TEXT", nullable: true),
                    isSeen = table.Column<bool>(type: "INTEGER", nullable: false),
                    createdDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    userId = table.Column<string>(type: "TEXT", nullable: true),
                    roomId = table.Column<Guid>(type: "TEXT", nullable: false)
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
                        name: "FK_chatMessages_chatRooms_roomId",
                        column: x => x.roomId,
                        principalTable: "chatRooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_userHistories_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "publicFacilities",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    typeOfPublic = table.Column<string>(type: "TEXT", nullable: true),
                    facilityCode = table.Column<string>(type: "TEXT", nullable: true),
                    BookingTaskID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicFacilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_publicFacilities_bookingTasks_BookingTaskID",
                        column: x => x.BookingTaskID,
                        principalTable: "bookingTasks",
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
                    blockId = table.Column<Guid>(type: "TEXT", nullable: false),
                    userHistoriesuserHistoryId = table.Column<Guid>(type: "TEXT", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_apartments_userHistories_userHistoriesuserHistoryId",
                        column: x => x.userHistoriesuserHistoryId,
                        principalTable: "userHistories",
                        principalColumn: "userHistoryId");
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
                name: "assignIssues",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    staffId = table.Column<string>(type: "TEXT", nullable: true),
                    IssueID = table.Column<Guid>(type: "TEXT", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "filePaths",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    filePath = table.Column<string>(type: "TEXT", nullable: true),
                    ownerID = table.Column<string>(type: "TEXT", nullable: true),
                    IssuesID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filePaths", x => x.ID);
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
                    priorityLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    createdDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    lastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    isPrivate = table.Column<bool>(type: "INTEGER", nullable: false),
                    authorId = table.Column<string>(type: "TEXT", nullable: false),
                    statusLookUplookUpID = table.Column<Guid>(type: "TEXT", nullable: true),
                    CategoryID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_issues_AspNetUsers_authorId",
                        column: x => x.authorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_issues_categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "categories",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "lookUp",
                columns: table => new
                {
                    lookUpID = table.Column<Guid>(type: "TEXT", nullable: false),
                    lookUpTypeName = table.Column<string>(type: "TEXT", nullable: true),
                    lookUpTypeCode = table.Column<string>(type: "TEXT", nullable: true),
                    index = table.Column<string>(type: "TEXT", nullable: true),
                    valueString = table.Column<string>(type: "TEXT", nullable: true),
                    IssuesID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lookUp", x => x.lookUpID);
                    table.ForeignKey(
                        name: "FK_lookUp_issues_IssuesID",
                        column: x => x.IssuesID,
                        principalTable: "issues",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsVoteUp = table.Column<bool>(type: "INTEGER", nullable: false),
                    issuesID = table.Column<Guid>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_votes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_votes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_votes_issues_issuesID",
                        column: x => x.issuesID,
                        principalTable: "issues",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_apartments_blockId",
                table: "apartments",
                column: "blockId");

            migrationBuilder.CreateIndex(
                name: "IX_apartments_userHistoriesuserHistoryId",
                table: "apartments",
                column: "userHistoriesuserHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRoom_usersId",
                table: "ApplicationUserRoom",
                column: "usersId");

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
                name: "IX_assignIssues_IssueID",
                table: "assignIssues",
                column: "IssueID",
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
                name: "IX_bookingTasks_userID",
                table: "bookingTasks",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_chatMessages_roomId",
                table: "chatMessages",
                column: "roomId");

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
                name: "IX_filePaths_IssuesID",
                table: "filePaths",
                column: "IssuesID");

            migrationBuilder.CreateIndex(
                name: "IX_issues_authorId",
                table: "issues",
                column: "authorId");

            migrationBuilder.CreateIndex(
                name: "IX_issues_CategoryID",
                table: "issues",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_issues_statusLookUplookUpID",
                table: "issues",
                column: "statusLookUplookUpID");

            migrationBuilder.CreateIndex(
                name: "IX_lookUp_IssuesID",
                table: "lookUp",
                column: "IssuesID");

            migrationBuilder.CreateIndex(
                name: "IX_publicFacilities_BookingTaskID",
                table: "publicFacilities",
                column: "BookingTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_rentalHistories_apartmentId",
                table: "rentalHistories",
                column: "apartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_rentalHistories_applicationUserId",
                table: "rentalHistories",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_userHistories_applicationUserId",
                table: "userHistories",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_votes_issuesID",
                table: "votes",
                column: "issuesID");

            migrationBuilder.CreateIndex(
                name: "IX_votes_UserId",
                table: "votes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_assignIssues_issues_IssueID",
                table: "assignIssues",
                column: "IssueID",
                principalTable: "issues",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_issues_issuesID",
                table: "comments",
                column: "issuesID",
                principalTable: "issues",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_filePaths_issues_IssuesID",
                table: "filePaths",
                column: "IssuesID",
                principalTable: "issues",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_issues_lookUp_statusLookUplookUpID",
                table: "issues",
                column: "statusLookUplookUpID",
                principalTable: "lookUp",
                principalColumn: "lookUpID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_issues_AspNetUsers_authorId",
                table: "issues");

            migrationBuilder.DropForeignKey(
                name: "FK_lookUp_issues_IssuesID",
                table: "lookUp");

            migrationBuilder.DropTable(
                name: "ApplicationUserRoom");

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
                name: "assignIssues");

            migrationBuilder.DropTable(
                name: "chatMessages");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "filePaths");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "publicFacilities");

            migrationBuilder.DropTable(
                name: "rentalHistories");

            migrationBuilder.DropTable(
                name: "socialRecognizations");

            migrationBuilder.DropTable(
                name: "votes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "chatRooms");

            migrationBuilder.DropTable(
                name: "bookingTasks");

            migrationBuilder.DropTable(
                name: "apartments");

            migrationBuilder.DropTable(
                name: "blocks");

            migrationBuilder.DropTable(
                name: "userHistories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "issues");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "lookUp");
        }
    }
}
