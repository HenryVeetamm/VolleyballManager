using App.DAL.Contracts.Repositories;
using Base.Contracts.DAL;

namespace App.DAL.Contracts;

public interface IAppUnitOfWork : IUnitOfWork
{
    IAnnouncementRepository Announcement { get; }
    IClubRepository Club { get; }
    IMatchRepository Match { get; }
    IPersonInClubRepository PersonInClub { get; }
    IPersonInMatchRepository PersonInMatch { get; }
    IPersonInTeamRepository PersonInTeam { get; }
    IPersonInWorkoutRepository PersonInWorkout { get; }
    IRolesInTeamRepository RolesInTeam { get; }
    ISavedComparisonRepository SavedComparison { get; }
    ITeamRepository Team { get; }
    IWorkoutRepository Workout { get; }
    IWorkoutTypeRepository WorkoutType { get; }
    IUserRepository Users { get; }
}