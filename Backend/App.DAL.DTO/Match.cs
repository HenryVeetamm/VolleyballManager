using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;


namespace App.DAL.DTO;

public class Match : DomainEntityId, IDomainAppUserId
{
    public Guid HomeTeamId { get; set; }
    public Team? HomeTeam { get; set; }

    public Guid AwayTeamId { get; set; }
    public Team? AwayTeam { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime MatchDate { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string MatchScore { get; set; } = null!;

    public ICollection<PersonInMatch>? PersonInMatches { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}