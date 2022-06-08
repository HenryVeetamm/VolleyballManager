using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.BLL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;


namespace App.BLL.DTO;

public class Team : DomainEntityId, IDomainAppUserId
{
    public Guid ClubId { get; set; }
    
    public Club? Club { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string Name { get; set; } = null!;

    [MaxLength(16)] [MinLength(3)]
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