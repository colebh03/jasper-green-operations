using JasperGreen.Models;
using Microsoft.AspNetCore.Identity;

namespace JasperGreen.Models
{
	public class ConfigureIdentity
	{
		public static async Task CreateAdminUserAsync(IServiceProvider provider)
		{
			var roleManager =
				provider.GetRequiredService<RoleManager<IdentityRole>>(); 
			var userManager =
				provider.GetRequiredService<UserManager<User>>();           

            var configuration = provider.GetRequiredService<IConfiguration>();

            string? username = configuration["AdminUser:Username"];
            string? password = configuration["AdminUser:Password"];
            string roleName = "Admin";

			// Skip admin-user creation when credentials are not configured
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            // Ensure the Admin role exists before creating the initial administrator account
            if (await roleManager.FindByNameAsync(roleName) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(roleName));
			}
			// Seed the configured admin account only if it does not exist
			if (await userManager.FindByNameAsync(username) == null)
			{
				User user = new User { UserName = username };
				var result = await userManager.CreateAsync(user, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, roleName);
				}
			}
		}
	}
} 
