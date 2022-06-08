using App.BLL.Contracts.Services;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using Base.BLL.Services;
using Base.Contracts.BLL.Mappers;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;

namespace App.BLL.Services;

public class MatchService :  BaseEntityService<IAppUnitOfWork, IMatchRepository, AppBllDTO.Match, AppDalDTO.Match> 
    , IMatchService
{
    public MatchService(IAppUnitOfWork serviceUow, IMatchRepository serviceRepository, IMapper mapper) 
        : base(serviceUow, serviceRepository, new MatchMapper(mapper))
    {
    }

    public async Task<IEnumerable<AppBllDTO.Match>> GetAllMatchesAsync(Guid userId = default, bool noTracking = true)
    {
        var matches = await ServiceRepository.GetAllAsync(userId, noTracking);

        var mappedMatches = matches.Select(x => Mapper.Map(x)).ToList();

        foreach (var match in mappedMatches)
        {
            match!.Victory = DetermineWinner(match!.MatchScore.Replace('"', ' ').Trim());
        }

        return mappedMatches!;
    }
    
    public static bool DetermineWinner(string gameScore)
    {
        try
        {
            var setList = gameScore.Split(",");

            var homeTeamSetCount = 0;
            var awayTeamSetCount = 0;

            foreach (var set in setList)
            {
                Console.WriteLine(set);
                var setPoints = set.Split(":");


                if (int.Parse(setPoints[0]) > int.Parse(setPoints[1]))
                {
                    homeTeamSetCount++;
                }
                else
                {
                    awayTeamSetCount++;
                }
            }

            return homeTeamSetCount > awayTeamSetCount;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}