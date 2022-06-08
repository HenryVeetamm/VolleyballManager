
using App.BLL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class PersonInTeam : DomainEntityId, IDomainAppUserId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid TeamId { get; set; }
    public Team? Team { get; set; }

    public Guid RolesInTeamId { get; set; }
    public RolesInTeam? RolesInTeam { get; set; }
}