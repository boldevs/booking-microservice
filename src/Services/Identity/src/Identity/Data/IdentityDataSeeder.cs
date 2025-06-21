using System;
using System.Threading.Tasks;
using BuldingBlock.EFCore; // This reference seems unused in this specific file, but might be for other IDataSeeder implementations or general context.
using Identity.Identity.Constants;
using Identity.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Data;

public class IdentityDataSeeder : IDataSeeder
{
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityDataSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<long>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAllAsync()
    {
        await SeedRoles();
        await SeedUsers();
    }

    private async Task SeedRoles()
    {
        // Check if Admin role exists, if not, create it
        if (await _roleManager.RoleExistsAsync(Constants.Role.Admin) == false)
            await _roleManager.CreateAsync(new(Constants.Role.Admin));

        // Check if User role exists, if not, create it
        if (await _roleManager.RoleExistsAsync(Constants.Role.User) == false)
            await _roleManager.CreateAsync(new(Constants.Role.User));
    }

    private async Task SeedUsers()
    {
        // Seed the 'meysamh' (Admin) user if they don't exist
        if (await _userManager.FindByNameAsync("meysamh") == null)
        {
            var user = new ApplicationUser
            {
                FirstName = "Meysam",
                LastName = "Hadeli",
                UserName = "meysamh",
                Email = "meysam@test.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                // FIX: Assign a non-null value to PassPortNumber
                PassPortNumber = "ADMINPPN12345"
            };

            // Attempt to create the user with the specified password
            var result = await _userManager.CreateAsync(user, "Admin@123456");

            // If user creation is successful, add them to the Admin role
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, Constants.Role.Admin);
            // Optionally, handle failure to create user (e.g., log errors)
        }

        // Seed the 'meysamh2' (User) user if they don't exist
        if (await _userManager.FindByNameAsync("meysamh2") == null)
        {
            var user = new ApplicationUser
            {
                FirstName = "Meysam",
                LastName = "Hadeli",
                UserName = "meysamh2",
                Email = "meysam2@test.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                // FIX: Assign a non-null value to PassPortNumber
                PassPortNumber = "USERPPN67890"
            };

            // Attempt to create the user with the specified password
            var result = await _userManager.CreateAsync(user, "User@123456");

            // If user creation is successful, add them to the User role
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, Constants.Role.User);
            // Optionally, handle failure to create user (e.g., log errors)
        }
    }
}
