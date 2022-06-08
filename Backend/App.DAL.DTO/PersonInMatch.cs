using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class PersonInMatch : DomainEntityId, IDomainAppUserId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid MatchId { get; set; }
    public Match? Match { get; set; }
    
    public int? TotalPoints { get; set; }
    public int? Aces { get; set; }
    public int? Faults { get; set; }
   
    public int? Reception { get; set; }
}