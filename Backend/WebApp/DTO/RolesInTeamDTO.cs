using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain;
using Base.Domain;

namespace WebApp.DTO;

public class RolesInTeamDTO : DomainEntityId
{
    [MaxLength(32)] [MinLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.RolesInTeam), Name = nameof(RoleDescription))]
    public string RoleDescription { get; set; } = null!;

    public ICollection<PersonInTeam>? PersonInTeams { get; set; }
}