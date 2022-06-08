using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.BLL.Services;
using App.DAL.Contracts;
using App.DAL.DTO.Identity;
using App.Domain;
using AutoMapper;
using Base.BLL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using AppUser = App.Domain.Identity.AppUser;
using PersonInClub = App.Resources.App.Domain.PersonInClub;

namespace App.BLL;

public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
{
    protected IMapper Mapper;
    

    public AppBLL(IAppUnitOfWork uow, IMapper mapper) : base(uow)
    {
        Mapper = mapper;
    }

    public IAnnouncementService Announcement => GetService<IAnnouncementService>(() 
        => new AnnouncementService(Uow, Uow.Announcement, Mapper));
    public IClubService Club => GetService<IClubService>(() => new ClubService(Uow, Uow.Club, Mapper));
    public IMatchService Match => GetService<IMatchService>(() => new MatchService(Uow, Uow.Match, Mapper));

    public IPersonInClubService PersonInClub =>
        GetService<IPersonInClubService>(() => new PersonInClubService(Uow, Uow.PersonInClub, Mapper));

    public IPersonInMatchService PersonInMatch => GetService<IPersonInMatchService>(
        () => new PersonInMatchService(Uow, Uow.PersonInMatch, Mapper));

    public IPersonInTeamService PersonInTeam => GetService<IPersonInTeamService>(
        () => new PersonInTeamService(Uow, Uow.PersonInTeam, Mapper));

    public IPersonInWorkoutService PersonInWorkout => GetService<IPersonInWorkoutService>(
        () => new PersonInWorkoutService(Uow, Uow.PersonInWorkout, Mapper));

    public IRolesInTeamService RolesInTeam => GetService<IRolesInTeamService>(
        () => new RolesInTeamService(Uow, Uow.RolesInTeam, Mapper));

    public ISavedComparisonService SavedComparison => GetService<ISavedComparisonService>(
        () => new SavedComparisonService(Uow, Uow.SavedComparison, Mapper));

    public ITeamService Team => GetService<ITeamService>(
        () => new TeamService(Uow, Uow.Team, Mapper));

    public IWorkoutService Workout => GetService<IWorkoutService>(
        () => new WorkoutService(Uow, Uow.Workout, Mapper));

    public IWorkoutTypeService WorkoutType => GetService<IWorkoutTypeService>(
        () => new WorkoutTypeService(Uow, Uow.WorkoutType, Mapper));

    public IUserService Users => GetService<IUserService>(
        () => new UserService(Uow, Uow.Users, Mapper));
}