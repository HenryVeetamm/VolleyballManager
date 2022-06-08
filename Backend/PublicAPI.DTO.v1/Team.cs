using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace PublicAPI.DTO.v1;

public class Team
{
    public Guid Id { get; set; }
    
    public Guid ClubId { get; set; }
    public Club? Club { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string Name { get; set; } = null!;

    [MaxLength(16)] [MinLength(3)]
    public string Code { get; set; } = null!;
    
    public bool OwnTeam { get; set; }
}

public class TeamUpdate
{
    public Guid Id { get; set; }
    
    public Guid ClubId { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string Name { get; set; } = null!;

    [MaxLength(16)] [MinLength(3)]
    public string Code { get; set; } = null!;
    
    public bool OwnTeam { get; set; }
}