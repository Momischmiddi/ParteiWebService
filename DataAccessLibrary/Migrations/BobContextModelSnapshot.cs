﻿// <auto-generated />
using System;
using DataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccessLibrary.Migrations
{
    [DbContext(typeof(BobContext))]
    partial class BobContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4");

            modelBuilder.Entity("DataAccessLibrary.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("MemberId")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<int>("OrgranizationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.ApplicationUserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.DataAccessLibrary.Models.Stop", b =>
                {
                    b.Property<int>("StopId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("StopName")
                        .HasColumnType("TEXT");

                    b.HasKey("StopId");

                    b.ToTable("Stop");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.ExternalMember", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<double>("ActualCosts")
                        .HasColumnType("REAL");

                    b.Property<string>("BoardingPoint")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Mail")
                        .HasColumnType("TEXT");

                    b.Property<string>("PreName")
                        .HasColumnType("TEXT");

                    b.Property<double>("TargetCosts")
                        .HasColumnType("REAL");

                    b.Property<int?>("TravelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("TravelId");

                    b.ToTable("ExternalMemebers");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageFileSize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageFileType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<int>("TravelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ImageId");

                    b.HasIndex("TravelId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.Member", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Adress")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("TEXT");

                    b.Property<double>("Contribution")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Home")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActiveMember")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("PreName")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ApplicationUserId")
                        .IsUnique();

                    b.HasIndex("OrganizationId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrganizationImage")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AdminId")
                        .IsUnique();

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.Travel", b =>
                {
                    b.Property<int>("TravelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Costs")
                        .HasColumnType("REAL");

                    b.Property<string>("Departure")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Destination")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxTraveler")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("TravelId");

                    b.ToTable("Travels");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.TravelMember", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<double>("ActualCosts")
                        .HasColumnType("REAL");

                    b.Property<string>("MemberID")
                        .HasColumnType("TEXT");

                    b.Property<int>("StopId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TargetCosts")
                        .HasColumnType("REAL");

                    b.Property<int?>("TravelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("MemberID");

                    b.HasIndex("StopId");

                    b.HasIndex("TravelId");

                    b.ToTable("TravelMembers");
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

                    b.ToTable("AspNetRoleClaims");
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

                    b.ToTable("AspNetUserClaims");
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

                    b.ToTable("AspNetUserLogins");
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

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.ApplicationUserRole", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.ApplicationRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLibrary.Models.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataAccessLibrary.Models.ExternalMember", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.Travel", null)
                        .WithMany("ExternalMembers")
                        .HasForeignKey("TravelId");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.Image", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.Travel", "Travel")
                        .WithMany("Images")
                        .HasForeignKey("TravelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataAccessLibrary.Models.Member", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("Member")
                        .HasForeignKey("DataAccessLibrary.Models.Member", "ApplicationUserId");

                    b.HasOne("DataAccessLibrary.Models.Organization", "Organization")
                        .WithMany("Members")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataAccessLibrary.Models.Organization", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.ApplicationUser", "Admin")
                        .WithOne("Organization")
                        .HasForeignKey("DataAccessLibrary.Models.Organization", "AdminId");
                });

            modelBuilder.Entity("DataAccessLibrary.Models.TravelMember", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberID");

                    b.HasOne("DataAccessLibrary.Models.DataAccessLibrary.Models.Stop", "Stop")
                        .WithMany()
                        .HasForeignKey("StopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLibrary.Models.Travel", "Travel")
                        .WithMany("TravelMembers")
                        .HasForeignKey("TravelId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DataAccessLibrary.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
