using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
	public class DepremAppContext: DbContext
	{
		public DepremAppContext(DbContextOptions<DepremAppContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Job> Jobs { get; set; }
		public DbSet<IdentityUserClaim<int>> UserClaims { get; set; }
		public DbSet<IdentityUserRole<int>> UserRoles { get; set; }
		public DbSet<IdentityRoleClaim<int>> AspNetRoleClaims { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>().HasData(
				new User
				{
					Id= 1,
					UserName = "DepremAppAdmin",
					NormalizedUserName= "DEPREMAPPADMIN",
					EmailConfirmed= false,
					PasswordHash= "AQAAAAEAACcQAAAAEDLbk863Dj38jtVwVt1TKltC8le+lswrsiH+i+vZscBsdwqlekyXoh1SklbZsL7e1Q==",
					SecurityStamp= "HSIGJTY7T6U45ALXA4JSEZAPBF67AM6V",
					ConcurrencyStamp= "ff99051d-f071-47c2-8883-580142cdb252",
					PhoneNumberConfirmed= false,
					TwoFactorEnabled= false,
					LockoutEnabled= false,
					AccessFailedCount= 0,
				});

			modelBuilder.Entity<Role>().HasData(
				new Role
				{
					Id= 1,
					Name="Admin",
					NormalizedName="ADMIN",
					ConcurrencyStamp= "f456259c-75e7-443a-86e8-a0d7aa019038"
				});

			modelBuilder.Entity<Role>().HasData(
				new Role
				{
					Id = 2,
					Name = "User",
					NormalizedName = "USER",
					ConcurrencyStamp = "567d3d4f-66d9-4eb8-85af-f05c31aed5cc"
				});

			modelBuilder.Entity<IdentityUserRole<int>>().HasData(

				new IdentityUserRole<int> {UserId=1,RoleId=1 });

			modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");

			modelBuilder.Entity<IdentityUserLogin<int>>(b =>
			{
				b.ToTable("UserLogins");
				b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
			});
			modelBuilder.Entity<IdentityUserRole<int>>(b =>
			{
				b.ToTable("UserRoles");
				b.HasKey(l => new { l.UserId, l.RoleId });
			});
			modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("AspNetRoleClaims");



		}

	}


}
