using App.Domain;
using App.Domain.Enums;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using RolesInTeam = App.Domain.RolesInTeam;

namespace DAL.App.EF.AppData;

public static class DataInitializer
{
    public static async void SeedAppData(AppDbContext ctx)
    {
        
        await SeedWorkoutTypes(ctx);
        await SeedRolesInTeam(ctx);

        await ctx.SaveChangesAsync();
    }

    public static async Task SeedWorkoutTypes(AppDbContext ctx)
    {
        foreach (var workoutTypeData in InitialData.WorkoutTypes)
        {
            await ctx.WorkoutTypes.AddAsync(new WorkoutType()
            {
                Description =
                {
                    ["en"] = workoutTypeData.en,
                    ["et"] = workoutTypeData.et
                }
            });
        }

        await ctx.SaveChangesAsync();
    }

    public static async Task SeedRolesInTeam(AppDbContext ctx)
    {
        foreach (var roleInfo in InitialData.RolesInTeams)
        {
            var roleInTeam = new RolesInTeam
            {
                RoleDescription =
                {
                    ["en"] = roleInfo.en,
                    ["et"] = roleInfo.et
                }
            };
            await ctx.RolesInTeams.AddAsync(roleInTeam);
        }

        await ctx.SaveChangesAsync();
    }


    
    
    public static void SeedRoles(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        foreach (var roleName in InitialData.Roles)
        {
            var role = roleManager.FindByNameAsync(roleName).Result;
            if (role == null)
            {
                role = new AppRole()
                {
                    Name = roleName
                };

                var result = roleManager.CreateAsync(role).Result;
            }
        }

        foreach (var userInfo in InitialData.Users)
        {
            var user = new AppUser()
            {
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                UserName = userInfo.email,
                Birthday = DateTime.Parse(userInfo.Birthday),
                Gender = userInfo.Gender,
                NationalCode = userInfo.nationalCode,
                Email = userInfo.email,
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(user, userInfo.password).Result;

            var userResult = userManager.AddToRoleAsync(user, userInfo.role).Result;
        }
    }
}