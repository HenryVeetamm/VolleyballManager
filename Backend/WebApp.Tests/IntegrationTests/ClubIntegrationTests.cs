using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App.Domain.Identity;
using DAL.App.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using PublicAPI.DTO.v1;
using PublicAPI.DTO.v1.Account;
using PublicAPI.DTO.v1.Enums;
using Xunit;
using Xunit.Abstractions;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace WebApp.Tests.IntegrationTests;

public class ClubIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;
    private readonly AppDbContext _ctx;
   


    public ClubIntegrationTests(CustomWebApplicationFactory<Program> factory,
        ITestOutputHelper testOutputHelper)
    {
        
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        
        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        optionBuilder.EnableSensitiveDataLogging();
        optionBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        _ctx = new AppDbContext(optionBuilder.Options);
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });
    }
    
    

    [Fact]
    public async Task Test_LoggedIn_User_Creates_A_Club()
    {
        await NotLoggedInUserTriesToCreateClub();

        await RegisterUser();

        var token = await LoginUser();

        //Should be empty
        await CheckAllUserClubs(token, 0);

        //Creates own team
        await UserCreatesClub(token, true);
        
        await CheckAllUserClubs(token, 1);
        
        //Creates opponent team
        await UserCreatesClub(token, false);

        await CheckAllUserClubs(token, 2);
        
        //Check own clubs

        await CheckUserOwnedClubs(token, "ownClubs", 1);
        //Check opponent clubs
        await CheckUserOwnedClubs(token, "opponentClubs", 1);
    }

    private async Task UserCreatesClub(string token, bool ownClub)
    {
        var club = new Club
        {
            Name = "Test",
            OwnClub = ownClub
        };
        var clubJson = System.Text.Json.JsonSerializer.Serialize(club);
        var apiRequest = new HttpRequestMessage(HttpMethod.Post, "api/v1.0/Club");
        apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        apiRequest.Content = new StringContent(clubJson, Encoding.UTF8, "application/json");
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var apiResponse = await _client.SendAsync(apiRequest);
        
        Assert.Equal(201, (int) apiResponse.StatusCode);
    }

    private async Task RegisterUser()
    {

        var registerDto = new Register()
        {
            Email = "test@test.ee",
            FirstName = "TestFirst",
            LastName = "TestLast",
            Birthday = new DateTime(1, 1, 1),
            Role = "Coach",
            Nationalcode = "00000000000",
            Password = "Admin,1"
        };

        var jsonStr = System.Text.Json.JsonSerializer.Serialize(registerDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/identity/account/register", data);

        response.EnsureSuccessStatusCode();
    }

    private async Task CheckUserOwnedClubs(string token, string path, int count)
    {
        var apiRequest = new HttpRequestMessage(HttpMethod.Get, $"api/v1.0/Club/{path}");
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var apiResponse = await _client.SendAsync(apiRequest);
        var clubs = await apiResponse.Content.ReadAsStringAsync();
        var resultData = System.Text.Json.JsonSerializer.Deserialize<List<Club>>(clubs);
        
        Assert.Equal(200, (int) apiResponse.StatusCode );
        Assert.Equal(count, resultData!.Count);
    }

    private async Task CheckAllUserClubs(string token, int count)
    {
        var apiRequest = new HttpRequestMessage(HttpMethod.Get, "api/v1.0/Club");
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var apiResponse = await _client.SendAsync(apiRequest);
        
        var clubs = await apiResponse.Content.ReadAsStringAsync();
        var resultData = System.Text.Json.JsonSerializer.Deserialize<List<Club>>(clubs);
        
        //Assert
        Assert.Equal(count, resultData!.Count);
    }

    private async Task<string> LoginUser()
    {
        var loginDto = new Login()
        {
            Email = "test@test.ee",
            Password = "Admin,1"
        };
        
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(loginDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/identity/account/login", data);
        var requestContent = await response.Content.ReadAsStringAsync();

        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );
        
        response.EnsureSuccessStatusCode();

        return resultJwt!.Token;
    }
    
    private async Task NotLoggedInUserTriesToCreateClub()
    {
        var club = new Club
        {
            Name = "Test",
            OwnClub = true
        };
        
        var clubJson = System.Text.Json.JsonSerializer.Serialize(club);
        var apiRequest = new HttpRequestMessage(HttpMethod.Post, "api/v1.0/Club");
        apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        apiRequest.Content = new StringContent(clubJson, Encoding.UTF8, "application/json");

        var apiResponse = await _client.SendAsync(apiRequest);
        
        Assert.Equal(401, (int) apiResponse.StatusCode);


    }
}