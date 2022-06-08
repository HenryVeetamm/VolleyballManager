using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.BLL.DTO;

public class RolesInTeam : DomainEntityId
{
    [MaxLength(32)] [MinLength(3)]
    [Column(TypeName = "jsonb")]
    public LangStr RoleDescription { get; set; } = new();

    public ICollection<PersonInTeam>? PersonInTeams { get; set; }
}