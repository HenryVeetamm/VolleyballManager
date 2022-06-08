
using Base.Contracts.Domain;
using Base.Domain;
using PublicAPI.DTO.v1.Identity;

namespace PublicAPI.DTO.v1;

public class PersonInMatch
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid MatchId { get; set; }
    public Match? Match { get; set; }
    
    public int? TotalPoints { get; set; }
    public int? Aces { get; set; }
    public int? Faults { get; set; }
   
    public int? Reception { get; set; }
}