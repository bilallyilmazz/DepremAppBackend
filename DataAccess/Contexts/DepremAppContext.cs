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
		public DbSet<IdentityUserLogin<int>> UserLogins { get; set; }
		public DbSet<IdentityUserClaim<int>> UserClaims { get; set; }
		public DbSet<IdentityUserRole<int>> UserRoles { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

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
