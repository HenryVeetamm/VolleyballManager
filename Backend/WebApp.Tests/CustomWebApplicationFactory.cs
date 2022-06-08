using System;
using System.Linq;
using App.Domain.Identity;
using DAL.App.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp.Tests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> 
where TStartup : class
{
    private static bool dbInitialized = false;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //find DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            
            //if found - remove
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            //and new DbContext
            services.AddDbContext<AppDbContext>(
                options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting"); 
                    
                });
            
            //data seeding
            //create db and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            db.Database.EnsureCreated();

            var userManage = scopedServices.GetService<RoleManager<AppRole>>();

            var role = new AppRole()
            {
                Name = "Coach"
            };
            userManage!.CreateAsync(role);

            try
            {
                if (dbInitialized == false)
                {
                    dbInitialized = true;
                    
                    
                }
            }
            catch (Exception e)
            {  
                logger.LogError(e, "An error occurred seeding the " +
                                   "database with test messages. Error: {Message}", e.Message);
            }

        });
    }
}