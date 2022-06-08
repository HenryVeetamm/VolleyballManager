
using Base.Contracts.Domain;
using Base.Domain;
using PublicAPI.DTO.v1.Identity;

namespace PublicAPI.DTO.v1;

public class PersonInTeam
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid TeamId { get; set; }
    public Team? Team { get; set; }

    public Guid RolesInTeamId { get; set; }
    public RolesInTeam? RolesInTeam { get; set; }
}