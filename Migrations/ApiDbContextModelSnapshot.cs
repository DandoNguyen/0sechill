﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using _0sechill.Data;

#nullable disable

namespace _0sechill.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    partial class ApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("_0sechill.Hubs.Model.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createdDateTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isSeen")
                        .HasColumnType("INTEGER");

                    b.Property<string>("message")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("roomId")
                        .HasColumnType("TEXT");

                    b.Property<string>("userId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("roomId");

                    b.HasIndex("userId");

                    b.ToTable("chatMessages");
                });

            modelBuilder.Entity("_0sechill.Hubs.Model.Notifications", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("content")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isSeen")
                        .HasColumnType("INTEGER");

                    b.Property<string>("receiverId")
                        .HasColumnType("TEXT");

                    b.Property<string>("title")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("notifications");
                });

            modelBuilder.Entity("_0sechill.Hubs.Model.Room", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("groupAdmin")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isGroupChat")
                        .HasColumnType("INTEGER");

                    b.Property<string>("roomName")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("chatRooms");
                });

            modelBuilder.Entity("_0sechill.Models.Account.Department", b =>
                {
                    b.Property<Guid>("departmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("departmentName")
                        .HasColumnType("TEXT");

                    b.HasKey("departmentId");

                    b.ToTable("departments");
                });

            modelBuilder.Entity("_0sechill.Models.Apartment", b =>
                {
                    b.Property<Guid>("apartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("apartmentName")
                        .HasColumnType("TEXT");

                    b.Property<int>("bedroomAmount")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("blockId")
                        .HasColumnType("TEXT");

                    b.Property<int>("clearanceArea")
                        .HasColumnType("INTEGER");

                    b.Property<int>("heartWallArea")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("userHistoriesuserHistoryId")
                        .HasColumnType("TEXT");

                    b.HasKey("apartmentId");

                    b.HasIndex("blockId");

                    b.HasIndex("userHistoriesuserHistoryId");

                    b.ToTable("apartments");
                });

            modelBuilder.Entity("_0sechill.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IDNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("IDType")
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TokenCreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TokenExpireDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<int>("age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("currentHubConnectionId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("departmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("firstName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isMale")
                        .HasColumnType("INTEGER");

                    b.Property<string>("lastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("phoneCountryCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("residentialAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("role")
                        .HasColumnType("TEXT");

                    b.Property<string>("roleID")
                        .HasColumnType("TEXT");

                    b.Property<string>("userCode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("departmentId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("_0sechill.Models.Block", b =>
                {
                    b.Property<Guid>("blockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("blockManagerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("blockName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("flourAmount")
                        .HasColumnType("INTEGER");

                    b.HasKey("blockId");

                    b.HasIndex("blockManagerId")
                        .IsUnique();

                    b.ToTable("blocks");
                });

            modelBuilder.Entity("_0sechill.Models.BookingTask", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("DateOfBooking")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PublicFacilityID")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("TimeLevelOfBooking")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isAvailable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("userID")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("PublicFacilityID");

                    b.HasIndex("userID");

                    b.ToTable("bookingTasks");
                });

            modelBuilder.Entity("_0sechill.Models.Infrastructure.PublicFacility", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("facilCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("typeFacil")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("publicFacilities");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.AssignIssue", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("IssueID")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isConfirmedByAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isResolved")
                        .HasColumnType("INTEGER");

                    b.Property<string>("staffFeedback")
                        .HasColumnType("TEXT");

                    b.Property<string>("staffId")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("IssueID")
                        .IsUnique();

                    b.HasIndex("staffId");

                    b.ToTable("assignIssues");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Category", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("cateName")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Comments", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("authorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("authorsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("content")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isChild")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isPrivate")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("issueId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("issuesID")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("parentId")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("authorsId");

                    b.HasIndex("issuesID");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.FilePath", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("IssuesID")
                        .HasColumnType("TEXT");

                    b.Property<string>("filePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("ownerID")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("IssuesID");

                    b.ToTable("filePaths");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Issues", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CategoryID")
                        .HasColumnType("TEXT");

                    b.Property<string>("authorId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("feedback")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isPrivate")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("lastModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("priorityLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("status")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("statusLookUplookUpID")
                        .HasColumnType("TEXT");

                    b.Property<string>("title")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("authorId");

                    b.HasIndex("statusLookUplookUpID");

                    b.ToTable("issues");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Vote", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsVoteUp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("issuesID")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.HasIndex("issuesID");

                    b.ToTable("votes");
                });

            modelBuilder.Entity("_0sechill.Models.LookUpData.LookUpTable", b =>
                {
                    b.Property<Guid>("lookUpID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("IssuesID")
                        .HasColumnType("TEXT");

                    b.Property<string>("index")
                        .HasColumnType("TEXT");

                    b.Property<string>("lookUpTypeCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("lookUpTypeName")
                        .HasColumnType("TEXT");

                    b.Property<string>("valueString")
                        .HasColumnType("TEXT");

                    b.HasKey("lookUpID");

                    b.HasIndex("IssuesID");

                    b.ToTable("lookUp");
                });

            modelBuilder.Entity("_0sechill.Models.RentalHistory", b =>
                {
                    b.Property<Guid>("rentalHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("apartmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("applicationUserId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("createdDate")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("endDate")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("lastSignedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("modifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("startDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("userId")
                        .HasColumnType("TEXT");

                    b.HasKey("rentalHistoryId");

                    b.HasIndex("apartmentId");

                    b.HasIndex("applicationUserId");

                    b.ToTable("rentalHistories");
                });

            modelBuilder.Entity("_0sechill.Models.SocialRecognization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("number")
                        .HasColumnType("TEXT");

                    b.Property<string>("type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("socialRecognizations");
                });

            modelBuilder.Entity("_0sechill.Models.UserHistory", b =>
                {
                    b.Property<Guid>("userHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("apartmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("applicationUserId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("createdDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("endDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("lastSignedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("modifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("startDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("status")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("userId")
                        .HasColumnType("TEXT");

                    b.HasKey("userHistoryId");

                    b.HasIndex("applicationUserId");

                    b.ToTable("userHistories");
                });

            modelBuilder.Entity("ApplicationUserRoom", b =>
                {
                    b.Property<Guid>("chatRoomsID")
                        .HasColumnType("TEXT");

                    b.Property<string>("usersId")
                        .HasColumnType("TEXT");

                    b.HasKey("chatRoomsID", "usersId");

                    b.HasIndex("usersId");

                    b.ToTable("ApplicationUserRoom");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("_0sechill.Hubs.Model.Message", b =>
                {
                    b.HasOne("_0sechill.Hubs.Model.Room", "Room")
                        .WithMany("messages")
                        .HasForeignKey("roomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_0sechill.Models.ApplicationUser", "User")
                        .WithMany("Messages")
                        .HasForeignKey("userId");

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("_0sechill.Models.Apartment", b =>
                {
                    b.HasOne("_0sechill.Models.Block", "block")
                        .WithMany("apartments")
                        .HasForeignKey("blockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_0sechill.Models.UserHistory", "userHistories")
                        .WithMany("apartment")
                        .HasForeignKey("userHistoriesuserHistoryId");

                    b.Navigation("block");

                    b.Navigation("userHistories");
                });

            modelBuilder.Entity("_0sechill.Models.ApplicationUser", b =>
                {
                    b.HasOne("_0sechill.Models.Account.Department", "department")
                        .WithMany("users")
                        .HasForeignKey("departmentId");

                    b.Navigation("department");
                });

            modelBuilder.Entity("_0sechill.Models.Block", b =>
                {
                    b.HasOne("_0sechill.Models.ApplicationUser", "blockManager")
                        .WithOne("block")
                        .HasForeignKey("_0sechill.Models.Block", "blockManagerId");

                    b.Navigation("blockManager");
                });

            modelBuilder.Entity("_0sechill.Models.BookingTask", b =>
                {
                    b.HasOne("_0sechill.Models.Infrastructure.PublicFacility", "PublicFacility")
                        .WithMany()
                        .HasForeignKey("PublicFacilityID");

                    b.HasOne("_0sechill.Models.ApplicationUser", "User")
                        .WithMany("bookingTasks")
                        .HasForeignKey("userID");

                    b.Navigation("PublicFacility");

                    b.Navigation("User");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.AssignIssue", b =>
                {
                    b.HasOne("_0sechill.Models.IssueManagement.Issues", "Issue")
                        .WithOne("assignIssue")
                        .HasForeignKey("_0sechill.Models.IssueManagement.AssignIssue", "IssueID");

                    b.HasOne("_0sechill.Models.ApplicationUser", "staff")
                        .WithMany("assignIssues")
                        .HasForeignKey("staffId");

                    b.Navigation("Issue");

                    b.Navigation("staff");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Comments", b =>
                {
                    b.HasOne("_0sechill.Models.ApplicationUser", "authors")
                        .WithMany("comments")
                        .HasForeignKey("authorsId");

                    b.HasOne("_0sechill.Models.IssueManagement.Issues", "issues")
                        .WithMany("comments")
                        .HasForeignKey("issuesID");

                    b.Navigation("authors");

                    b.Navigation("issues");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.FilePath", b =>
                {
                    b.HasOne("_0sechill.Models.IssueManagement.Issues", null)
                        .WithMany("files")
                        .HasForeignKey("IssuesID");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Issues", b =>
                {
                    b.HasOne("_0sechill.Models.IssueManagement.Category", null)
                        .WithMany("Issues")
                        .HasForeignKey("CategoryID");

                    b.HasOne("_0sechill.Models.ApplicationUser", "author")
                        .WithMany("issues")
                        .HasForeignKey("authorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_0sechill.Models.LookUpData.LookUpTable", "statusLookUp")
                        .WithMany()
                        .HasForeignKey("statusLookUplookUpID");

                    b.Navigation("author");

                    b.Navigation("statusLookUp");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Vote", b =>
                {
                    b.HasOne("_0sechill.Models.ApplicationUser", "User")
                        .WithMany("votes")
                        .HasForeignKey("UserId");

                    b.HasOne("_0sechill.Models.IssueManagement.Issues", "issues")
                        .WithMany("votes")
                        .HasForeignKey("issuesID");

                    b.Navigation("User");

                    b.Navigation("issues");
                });

            modelBuilder.Entity("_0sechill.Models.LookUpData.LookUpTable", b =>
                {
                    b.HasOne("_0sechill.Models.IssueManagement.Issues", null)
                        .WithMany("listCateLookUp")
                        .HasForeignKey("IssuesID");
                });

            modelBuilder.Entity("_0sechill.Models.RentalHistory", b =>
                {
                    b.HasOne("_0sechill.Models.Apartment", "apartment")
                        .WithMany()
                        .HasForeignKey("apartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_0sechill.Models.ApplicationUser", "applicationUser")
                        .WithMany()
                        .HasForeignKey("applicationUserId");

                    b.Navigation("apartment");

                    b.Navigation("applicationUser");
                });

            modelBuilder.Entity("_0sechill.Models.UserHistory", b =>
                {
                    b.HasOne("_0sechill.Models.ApplicationUser", "applicationUser")
                        .WithMany("userHistories")
                        .HasForeignKey("applicationUserId");

                    b.Navigation("applicationUser");
                });

            modelBuilder.Entity("ApplicationUserRoom", b =>
                {
                    b.HasOne("_0sechill.Hubs.Model.Room", null)
                        .WithMany()
                        .HasForeignKey("chatRoomsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_0sechill.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("usersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("_0sechill.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("_0sechill.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_0sechill.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("_0sechill.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("_0sechill.Hubs.Model.Room", b =>
                {
                    b.Navigation("messages");
                });

            modelBuilder.Entity("_0sechill.Models.Account.Department", b =>
                {
                    b.Navigation("users");
                });

            modelBuilder.Entity("_0sechill.Models.ApplicationUser", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("assignIssues");

                    b.Navigation("block");

                    b.Navigation("bookingTasks");

                    b.Navigation("comments");

                    b.Navigation("issues");

                    b.Navigation("userHistories");

                    b.Navigation("votes");
                });

            modelBuilder.Entity("_0sechill.Models.Block", b =>
                {
                    b.Navigation("apartments");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Category", b =>
                {
                    b.Navigation("Issues");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Issues", b =>
                {
                    b.Navigation("assignIssue");

                    b.Navigation("comments");

                    b.Navigation("files");

                    b.Navigation("listCateLookUp");

                    b.Navigation("votes");
                });

            modelBuilder.Entity("_0sechill.Models.UserHistory", b =>
                {
                    b.Navigation("apartment");
                });
#pragma warning restore 612, 618
        }
    }
}
