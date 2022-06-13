﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using _0sechill.Data;

#nullable disable

namespace _0sechill.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20220613085016_update filePath table")]
    partial class updatefilePathtable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

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

                    b.HasKey("apartmentId");

                    b.HasIndex("blockId");

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

                    b.Property<DateOnly>("DOB")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

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

                    b.Property<string>("firstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("lastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("role")
                        .HasColumnType("TEXT");

                    b.Property<string>("userCode")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("userId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("_0sechill.Models.Block", b =>
                {
                    b.Property<Guid>("blockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("blockName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("flourAmount")
                        .HasColumnType("INTEGER");

                    b.HasKey("blockId");

                    b.ToTable("blocks");
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

            modelBuilder.Entity("_0sechill.Models.IssueManagement.FilePath", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("filePath")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("issueId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("issuesID")
                        .HasColumnType("TEXT");

                    b.Property<string>("userId")
                        .HasColumnType("TEXT");

                    b.Property<string>("usersId")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("issuesID");

                    b.HasIndex("usersId");

                    b.ToTable("filePaths");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Issues", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("authorId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("cateId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("categoryID")
                        .HasColumnType("TEXT");

                    b.Property<string>("content")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("createdDate")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("lastModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("privacy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("status")
                        .HasColumnType("TEXT");

                    b.Property<string>("title")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("authorId");

                    b.HasIndex("categoryID");

                    b.ToTable("issues");
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

                    b.HasKey("userHistoryId");

                    b.HasIndex("apartmentId");

                    b.HasIndex("applicationUserId");

                    b.ToTable("userHistories");
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

            modelBuilder.Entity("_0sechill.Models.Apartment", b =>
                {
                    b.HasOne("_0sechill.Models.Block", "block")
                        .WithMany("apartments")
                        .HasForeignKey("blockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("block");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.FilePath", b =>
                {
                    b.HasOne("_0sechill.Models.IssueManagement.Issues", "issues")
                        .WithMany("files")
                        .HasForeignKey("issuesID");

                    b.HasOne("_0sechill.Models.ApplicationUser", "users")
                        .WithMany()
                        .HasForeignKey("usersId");

                    b.Navigation("issues");

                    b.Navigation("users");
                });

            modelBuilder.Entity("_0sechill.Models.IssueManagement.Issues", b =>
                {
                    b.HasOne("_0sechill.Models.ApplicationUser", "author")
                        .WithMany("issues")
                        .HasForeignKey("authorId");

                    b.HasOne("_0sechill.Models.IssueManagement.Category", "category")
                        .WithMany("Issues")
                        .HasForeignKey("categoryID");

                    b.Navigation("author");

                    b.Navigation("category");
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
                    b.HasOne("_0sechill.Models.Apartment", "apartment")
                        .WithMany("userHistories")
                        .HasForeignKey("apartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_0sechill.Models.ApplicationUser", "applicationUser")
                        .WithMany("userHistories")
                        .HasForeignKey("applicationUserId");

                    b.Navigation("apartment");

                    b.Navigation("applicationUser");
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

            modelBuilder.Entity("_0sechill.Models.Apartment", b =>
                {
                    b.Navigation("userHistories");
                });

            modelBuilder.Entity("_0sechill.Models.ApplicationUser", b =>
                {
                    b.Navigation("issues");

                    b.Navigation("userHistories");
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
                    b.Navigation("files");
                });
#pragma warning restore 612, 618
        }
    }
}
