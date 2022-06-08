using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class RolesInTeam : DomainEntityId
{
    [MaxLength(32)] [MinLength(3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.RolesInTeam), Name = nameof(RoleDescription))]
    [Column(TypeName = "jsonb")]
    public LangStr RoleDescription { get; set; } = new();

    public ICollection<PersonInTeam>? PersonInTeams { get; set; }
}