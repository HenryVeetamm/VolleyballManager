using App.Domain;
using App.Domain.Identity;
using DAL.App.EF;
using DAL.App.EF.AppData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp;

public static class AppDataHelper
{
    public static void SetUpAppData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    { 
        var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

        if (context == null)
        {
            throw new ApplicationException("Problem in services, No db context");
        }
        
        if (configuration.GetValue<bool>("DataInitialization:All"))
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();
            DataInitializer.SeedRoles(userManager!, roleManager!);
            DataInitializer.SeedAppData(context);
            return;
        }
        
        if (configuration.GetValue<bool>("DataInitialization:DropDatabase"))
        {
            context.Database.EnsureDeleted();
        }

        if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase"))
        {
            context.Database.Migrate();
        }
        if (configuration.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();
            DataInitializer.SeedRoles(userManager!, roleManager!);
         
        }
        if (configuration.GetValue<bool>("DataInitialization:SeedData"))
        {
            DataInitializer.SeedAppData(context);
        }
    }
}