using App.DAL.Contracts;
using App.DAL.Contracts.Repositories;
using App.Domain.Identity;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using DAL.App.EF.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DAL.App.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUnitOfWork
{
    protected IMapper Mapper;
    

    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        Mapper = mapper;
        
    }

    public IAnnouncementRepository Announcement => GetRepository(() => new AnnouncementRepository(UOWDbContext, Mapper));
    public IClubRepository Club => GetRepository(() => new ClubRepository(UOWDbContext, Mapper));
    public IMatchRepository Match => GetRepository(() => new MatchRepository(UOWDbContext, Mapper));
    public IPersonInClubRepository PersonInClub => GetRepository(() => new PersonInClubRepository(UOWDbContext, Mapper));
    public IPersonInMatchRepository PersonInMatch => GetRepository(() => new PersonInMatchRepository(UOWDbContext, Mapper));
    public IPersonInTeamRepository PersonInTeam => GetRepository(() => new PersonInTeamRepository(UOWDbContext, Mapper));

    public IPersonInWorkoutRepository PersonInWorkout =>
        GetRepository(() => new PersonInWorkoutRepository(UOWDbContext, Mapper));

    public IRolesInTeamRepository RolesInTeam => GetRepository(() => new RolesInTeamRepository(UOWDbContext, Mapper));

    public ISavedComparisonRepository SavedComparison =>
        GetRepository(() => new SavedComparisonRepository(UOWDbContext, Mapper));

    public ITeamRepository Team => GetRepository(() => new TeamRepository(UOWDbContext, Mapper));
    public IWorkoutRepository Workout => GetRepository(() => new WorkoutRepository(UOWDbContext, Mapper));
    public IWorkoutTypeRepository WorkoutType => GetRepository(() => new WorkoutTypeRepository(UOWDbContext, Mapper));

    public IUserRepository Users => GetRepository(() => new UserRepository(UOWDbContext, Mapper));
}