using System;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.DTO;
using App.BLL.Services;
using AutoMapper;
using Base.BLL.Services;
using DAL.App.EF;
using DAL.App.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace WebApp.Tests.UnitTests;

public class ClubServiceUnitTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AppDbContext _ctx;
    private readonly ClubService _clubService;


    public ClubServiceUnitTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        _ctx = new AppDbContext(optionsBuilder.Options);
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<App.BLL.DTO.MappingProfiles.AutoMapperProfile>();
            cfg.AddProfile<App.DAL.DTO.MappingProfiles.AutoMapperProfile>();
            cfg.AddProfile<PublicAPI.DTO.v1.MappingProfiles.AutoMapperProfile>();
        }).CreateMapper();
        
        
        var uow = new AppUOW(_ctx, mapper);
        var repo = new ClubRepository(_ctx, mapper);

        _clubService = new ClubService(uow, repo, mapper);
    }

    private async Task SeedData(int num)
    {
        var user = new App.Domain.Identity.AppUser
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                FirstName = "Firstname 1",
                LastName = "LastName 1",
                Birthday = new DateTime(1, 1, 1),
                Gender = App.Domain.Enums.EGender.Male,
                NationalCode = "000000000"
            };
        await _ctx.Users.AddAsync(user);
        for (var i = 0; i < num; i++)
        {
            var club = new App.Domain.Club()
            {
                Id = new Guid($"00000000-0000-0000-0000-00000000000{i+1}"),
                Name = $"Name {i + 1}",
                OwnClub = true,
                AppUserId = user.Id,
                AppUser = user
            };
            await _ctx.Clubs.AddAsync(club);
        }
        
        for (var i = 7; i < 9; i++)
        {
            var club = new App.Domain.Club()
            {
                Id = new Guid($"00000000-0000-0000-0000-00000000000{i+1}"),
                Name = $"Name {i + 1}",
                OwnClub = false,
                AppUserId = user.Id,
                AppUser = user
                
            };
            
            await _ctx.Clubs.AddAsync(club);
        }

        await _ctx.SaveChangesAsync();
    }

    [Theory]
    [InlineData(5)]
    public async Task Test_GetUserOwnedClubs_Returns_Clubs(int num)
    {
        //Arrange
        await SeedData(num);

        //Act
        var result = (await _clubService.GetUserOwnedClubs(new Guid("00000000-0000-0000-0000-000000000002"))).ToList();

        //Assert
        foreach (var club in result)
        {
            Assert.True(club.OwnClub);
        }
        Assert.Equal(num, result.Count);
        Assert.NotNull(result);
        result.Should().AllBeOfType(typeof(Club));
    }
    
    [Theory]
    [InlineData(5)]
    public async Task Test_GetUserOpponentClubs_Returns_Clubs(int num)
    {
        //Arrange
        await SeedData(num);

        //Act
        var result = (await _clubService.GetUserOpponentClubs(new Guid("00000000-0000-0000-0000-000000000002"))).ToList();

        //Assert
        foreach (var club in result)
        {
            Assert.False(club.OwnClub);
        }
        Assert.Equal(2, result.Count);
        Assert.NotNull(result);
        result.Should().AllBeOfType(typeof(Club));
    }
}