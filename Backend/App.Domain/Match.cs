using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Match : DomainEntityId, IDomainAppUserId
{
    public Guid HomeTeamId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Match), Name = nameof(HomeTeam))]
    public Team? HomeTeam { get; set; }

    public Guid AwayTeamId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Match), Name = nameof(AwayTeam))]
    public Team? AwayTeam { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Match), Name = nameof(MatchDate))]
    public DateTime MatchDate { get; set; }

    [MaxLength(64)] [MinLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Match), Name = nameof(MatchScore))]
    public string MatchScore { get; set; } = null!;

    public ICollection<PersonInMatch>? PersonInMatches { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}