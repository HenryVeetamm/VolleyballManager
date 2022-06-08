using App.BLL.Contracts.Services;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories;
using AutoMapper;
using Base.BLL.Services;

using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;

namespace App.BLL.Services;

public class PersonInMatchService: BaseEntityService<IAppUnitOfWork, IPersonInMatchRepository, AppBllDTO.PersonInMatch, AppDalDTO.PersonInMatch> 
    , IPersonInMatchService
{
    public PersonInMatchService(IAppUnitOfWork serviceUow, IPersonInMatchRepository serviceRepository,IMapper mapper) 
        : base(serviceUow, serviceRepository, new PersonInMatchMapper(mapper))
    {
    }


    public async Task<IEnumerable<AppBllDTO.PersonInMatch>> GetAllDetailedPersonInMatch(Guid userId, bool noTracking = true)
    {
        var personMatches = (await GetUserPersonInMatches(userId, noTracking)).ToList();

        foreach (var entry in personMatches)
        { 
            Console.WriteLine(entry!.Match!.MatchScore);
            entry!.Match!.Victory = DetermineWinner(entry!.Match!.MatchScore.Replace('"', ' ').Trim());
        }

        return personMatches;
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

    public async Task<IEnumerable<AppBllDTO.PersonInMatch>> GetUserPersonInMatches(Guid userId, bool noTracking = true)
    {
        return (await ServiceRepository.GetUserPersonInMatches(userId, noTracking))
            .Select(x => Mapper.Map(x)!)
            .ToList();
    }

    public async Task<IEnumerable<AppBllDTO.PersonInMatch>> GetAllPersonInMatchByMatchId(Guid matchId,  bool noTracking = true)
    {
        return (await ServiceRepository.GetAllPersonInMatchByMatchId(matchId, noTracking))
            .Select(x => Mapper.Map(x)!)
            .ToList();
    }
}