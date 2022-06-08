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

namespace WebApp.Tests.UnitTests;

public class BaseServiceUnitTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AppDbContext _ctx;
    private readonly BaseEntityService<AppUOW, WorkoutRepository, Workout, App.DAL.DTO.Workout> _baseService;

    public BaseServiceUnitTests(ITestOutputHelper testOutputHelper)
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

        var baseMapper = new BaseMapper<App.BLL.DTO.Workout, App.DAL.DTO.Workout>(mapper);


        var uow = new AppUOW(_ctx, mapper);
        var repo = new WorkoutRepository(_ctx, mapper);

        _baseService = new BaseEntityService<AppUOW, WorkoutRepository, Workout, App.DAL.DTO.Workout>(
            uow, repo, baseMapper);
    }

    private async Task SeedData(int num)
    {
        for (var i = 0; i < num; i++)
        {
            var workoutType = new App.Domain.WorkoutType()
            {
                Description = $"Type description {i + 1}"
            };

            var workout = new App.Domain.Workout()
            {
                Id = new Guid($"00000000-0000-0000-0000-00000000000{i + 1}"),
                WorkoutType = workoutType,
                Description = $"Workout description {i + 1}",
                Date = new DateTime(),
            };
            await _ctx.Workouts.AddAsync(workout);
        }

        await _ctx.SaveChangesAsync();
    }

    [Theory]
    [InlineData(3)]
    public async Task Test_GetAllAsync_Returns_Workouts_With_Types(int num)
    {
        //Arrange
        await SeedData(num);

        //Act
        var result = (await _baseService.GetAllAsync()).ToList();

        //Assert
        Assert.Equal(num, result.Count!);
        Assert.NotNull(result);
        result.Should().AllBeOfType(typeof(Workout));
    }

    [Theory]
    [InlineData(3)]
    public async Task Test_FirstOrDefaultAsync_Returns_Workout_With_Type(int num)
    {
        //Arrange
        await SeedData(num);

        //Act
        var workoutFirst = await _baseService.FirstOrDefaultAsync(new Guid("00000000-0000-0000-0000-000000000001"));
        var workoutLast = await _baseService.FirstOrDefaultAsync(new Guid($"00000000-0000-0000-0000-00000000000{num}"));
        var workoutNull =
            await _baseService.FirstOrDefaultAsync(new Guid($"00000000-0000-0000-0000-00000000000{num + 1}"));

        //Assert
        workoutFirst.Should().NotBeNull().And.BeOfType<Workout>();
        workoutLast.Should().NotBeNull().And.BeOfType<Workout>();
        workoutNull.Should().BeNull();

        workoutFirst!.Description.Should().Be("Workout description 1");
        workoutLast!.Description.Should().Be($"Workout description {num}");
    }

    [Theory]
    [InlineData(3)]
    public async Task Test_Add_Returns_Workout_With_Type(int num)
    {
        //Arrange
        var workout = new Workout()
        {
            Description = "Workout add",
            Date = new DateTime(1, 1, 1),
            WorkoutType = new WorkoutType()
            {
                Description = "Workout type add"
            }
        };
        //Act

        var result = _baseService.Add(workout);
        await _ctx.SaveChangesAsync();
        var entityWorkout = await _ctx.Workouts.FirstOrDefaultAsync(x => x.Id == result.Id);

        //Assert
        result.Should().NotBeNull();
        result.Description.Should().Be("Workout add");
        entityWorkout.Should().NotBeNull();
        entityWorkout!.Description.Should().Be("Workout add");
    }

    [Fact]
    public async Task? Test_Update_Returns_Error_When_Not_Existing()
    {
        var workout = new Workout()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            Description = "Workout add",
            Date = new DateTime(1, 1, 1),
            WorkoutType = new WorkoutType()
            {
                Description = "Workout type add"
            }
        };

        var res = _baseService.Update(workout);
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