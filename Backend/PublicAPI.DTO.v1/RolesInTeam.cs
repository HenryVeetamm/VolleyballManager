using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace PublicAPI.DTO.v1;

public class RolesInTeam
{
    public Guid Id { get; set; }
    [MaxLength(32)] [MinLength(3)]
    [Column(TypeName = "jsonb")]
    public string RoleDescription { get; set; } = default!;
}