using System.ComponentModel.DataAnnotations;

using Base.Domain;


namespace PublicAPI.DTO.v1;

public class Match
{
    public Guid Id { get; set; }
    public Guid HomeTeamId { get; set; }
    public Team? HomeTeam { get; set; }

    public Guid AwayTeamId { get; set; }
    public Team? AwayTeam { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime MatchDate { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string MatchScore { get; set; } = null!;

    public bool Victory { get; set; }
}

public class MatchUpdate
{
    public Guid Id { get; set; }
    
    public Guid HomeTeamId { get; set; }

    public Guid AwayTeamId { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime MatchDate { get; set; }

    [MaxLength(64)] [MinLength(3)]
    public string MatchScore { get; set; } = null!;

    public bool Victory { get; set; }
}