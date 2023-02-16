using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
	public static class SeedData
	{
		public static async Task InitializeAsync(IServiceProvider services)
		{
			var userManager = services.GetRequiredService<UserManager<User>>();
			var roleManager = services.GetRequiredService<RoleManager<Role>>();

			string userName = "DepremAppAdmin";
			string password = "1q2w3e4r5T!";

			if (await roleManager.FindByNameAsync("Admin") == null)
			{
				await roleManager.CreateAsync(new Role() { Name="Admin"});
			}
			if (await roleManager.FindByNameAsync("User") == null)
			{
				await roleManager.CreateAsync(new Role() { Name = "User" });
			}

			if (await userManager.FindByNameAsync(userName) == null)
			{
				var user = new User { UserName = userName};
				var result = await userManager.CreateAsync(user, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "Admin");
				}
			}
		}
	}
}
