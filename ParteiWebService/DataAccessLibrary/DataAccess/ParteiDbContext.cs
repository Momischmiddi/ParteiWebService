﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataAccess
{
    public class ParteiDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ParteiDbContext(DbContextOptions<ParteiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Travel> Travels { get; set; }
        public DbSet<TravelMember> TravelMembers { get; set; }
        public DbSet<ExternalMember> ExternalMembers { get; set; }
        public DbSet<ExternalTravelMember> ExternalTravelMembers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<TravelStop> TravelStops { get; set; }

        public object Member { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TravelStop>(travelStops =>
            {
                travelStops.HasKey(ts => new { ts.StopId, ts.TravelId });

                travelStops.HasOne(ts => ts.Travel)
                    .WithMany(s => s.TravelStops)
                    .HasForeignKey(ts => ts.TravelId)
                    .IsRequired();

                travelStops.HasOne(ts => ts.Stop)
                    .WithMany(t => t.TravelStops)
                    .HasForeignKey(ts => ts.StopId)
                    .IsRequired();
            });

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            builder.Entity<Member>()
        .HasOne(a => a.ApplicationUser).WithOne(b => b.Member)
        .HasForeignKey<Member>(e => e.ApplicationUserId);

         builder.Entity<Organization>()
        .HasOne(a => a.Admin).WithOne(b => b.Organization)
        .HasForeignKey<Organization>(e => e.AdminId);
        }
    }
}
