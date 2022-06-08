using System;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.DTO;
using App.Domain.Enums;
using App.Domain.Identity;
using AutoMapper;
using Base.BLL.Mappers;
using Base.BLL.Services;
using DAL.App.EF;
using DAL.App.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
using AppUser = App.BLL.DTO.Identity.AppUser;
using EGender = App.BLL.DTO.Enums.EGender;

namespace WebApp.Tests.UnitTests;

public class BaseServiceUnitTests2
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AppDbContext _ctx;
    private readonly BaseEntityService<AppUOW, ClubRepository, Club, App.DAL.DTO.Club> _baseService;

    public BaseServiceUnitTests2(ITestOutputHelper testOutputHelper)
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

        var baseMapper = new BaseMapper<App.BLL.DTO.Club, App.DAL.DTO.Club>(mapper);


        var uow = new AppUOW(_ctx, mapper);
        var repo = new ClubRepository(_ctx, mapper);

        _baseService = new BaseEntityService<AppUOW, ClubRepository, Club, App.DAL.DTO.Club>(
            uow, repo, baseMapper);
    }

    private async Task SeedData(int num)
    {
        var user = new App.Domain.Identity.AppUser
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                FirstName = "Firstname 1",
                LastName = "LastName 1",
                Birthday = new DateTime(1, 1, 1),
                Gender = App.Domain.Enums.EGender.Male,
                NationalCode = "000000000"
            };
        for (var i = 0; i < num; i++)
        {
            var club = new App.Domain.Club()
            {
                Id = new Guid($"00000000-0000-0000-0000-00000000000{i+1}"),
                Name = $"Name {i + 1}",
                OwnClub = (i % 2 == 0),
                AppUserId = user.Id,
                AppUser = user
                
            };
            
            await _ctx.Clubs.AddAsync(club);
        }

        await _ctx.SaveChangesAsync();
    }

    [Theory]
    [InlineData(5)]
    public async Task Test_GetAllAsync_Returns_Clubs(int num)
    {
        //Arrange
        await SeedData(num);

        //Act
        var result = (await _baseService.GetAllAsync()).ToList();

        //Assert
        Assert.Equal(num, result.Count!);
        Assert.NotNull(result);
        result.Should().AllBeOfType(typeof(Club));
    }

    [Theory]
    [InlineData(3)]
    public async Task Test_FirstOrDefaultAsync_Returns_Club(int num)
    {
        //Arrange
        await SeedData(num);
    
        //Act
        var clubFirst = await _baseService.FirstOrDefaultAsync(new Guid("00000000-0000-0000-0000-000000000001"));
        var clubLast = await _baseService.FirstOrDefaultAsync(new Guid($"00000000-0000-0000-0000-00000000000{num}"));
        var clubNull =
            await _baseService.FirstOrDefaultAsync(new Guid($"00000000-0000-0000-0000-00000000000{num + 1}"));
    
        //Assert
        clubFirst.Should().NotBeNull().And.BeOfType<Club>();
        clubLast.Should().NotBeNull().And.BeOfType<Club>();
        clubNull.Should().BeNull();
    
        clubFirst!.Name.Should().Be("Name 1");
        clubLast!.Name.Should().Be($"Name {num}");
    }
    
    [Fact]
    public async Task Test_Add_Returns_Club()
    {
        //Arrange
        var user = new App.BLL.DTO.Identity.AppUser
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            FirstName = "Firstname 1",
            LastName = "LastName 1",
            Birthday = new DateTime(1, 1, 1),
            Gender = EGender.Male,
            NationalCode = "000000000"
        };
        var club = new Club()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            Name = "Name 1",
            OwnClub = false,
            AppUser = user
        };
        //Act
    
        var result = _baseService.Add(club);
        await _ctx.SaveChangesAsync();
        var entityClub = await _ctx.Clubs.FirstOrDefaultAsync(x => x.Id == result.Id);
    
        //Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Name 1");
        entityClub.Should().NotBeNull();
        entityClub!.Name.Should().Be("Name 1");
    }
    
    [Fact]
    public async Task? Test_Update_Returns_Error_When_Not_Existing()
    {
        var club = new Club()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            Name = "Name 1"
        };
    
        var res = _baseService.Update(club);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _ctx.SaveChangesAsync());
    }
    
    [Fact]
    public async Task Test_RemoveAsync__Throws_When_BadId()
    {
        await SeedData(3);
    
        await Assert.ThrowsAnyAsync<System.NullReferenceException>(() =>
            _baseService.RemoveAsync(new Guid("00000000-0000-0000-0000-000000000004")));
    }
    
    [Fact]
    public async Task Test_ExistsAsync_Returns_Correct_Boolean()
    {
        await SeedData(3);
    
        var resTrue = await _baseService.ExistsAsync(new Guid("00000000-0000-0000-0000-000000000003"));
        var resFalse = await _baseService.ExistsAsync(new Guid("00000000-0000-0000-0000-000000000004"));
    
        Assert.True(resTrue);
        Assert.False(resFalse);
    }
}