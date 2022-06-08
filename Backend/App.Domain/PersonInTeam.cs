using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(TeamId), nameof(AppUserId), IsUnique = true)]
public class PersonInTeam : DomainEntityId, IDomainAppUserId
{
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInTeam), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }

    public Guid TeamId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInTeam), Name = nameof(Team))]
    public Team? Team { get; set; }

    public Guid RolesInTeamId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.PersonInTeam), Name = nameof(RolesInTeam))]
    public RolesInTeam? RolesInTeam { get; set; }
}