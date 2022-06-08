using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Team : DomainEntityId, IDomainAppUserId
{
    public Guid ClubId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Team), Name = nameof(Club))]
    public Club? Club { get; set; }

    [MaxLength(64)] [MinLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Team), Name = nameof(Name))]
    public string Name { get; set; } = null!;

    [MaxLength(16)] [MinLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Team), Name = nameof(Code))]
    public string Code { get; set; } = null!;
    public bool OwnTeam { get; set; }

    public ICollection<Announcement>? Announcements { get; set; }

    public ICollection<PersonInTeam>? PersonInTeams { get; set; }

    [InverseProperty(nameof(Match.HomeTeam))]
    public ICollection<Match>? HomeTeamMatches { get; set; }
    
    [InverseProperty(nameof(Match.AwayTeam))]
    public ICollection<Match>? AwayTeamMatches { get; set; }


    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
        
}