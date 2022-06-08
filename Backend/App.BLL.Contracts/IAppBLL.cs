using App.BLL.Contracts.Services;
using Base.Contracts.BLL;

namespace App.BLL.Contracts;

public interface IAppBLL : IBaseBLL
{
    IAnnouncementService Announcement { get; }
    IClubService Club { get; }
    IMatchService Match { get; }
    IPersonInClubService PersonInClub { get; }
    IPersonInMatchService PersonInMatch { get; }
    IPersonInTeamService PersonInTeam { get; }
    IPersonInWorkoutService PersonInWorkout { get; }
    IRolesInTeamService RolesInTeam { get; }
    ISavedComparisonService SavedComparison { get; }
    ITeamService Team { get; }
    IWorkoutService Workout { get; }
    IWorkoutTypeService WorkoutType { get; }
    IUserService Users { get; }
}