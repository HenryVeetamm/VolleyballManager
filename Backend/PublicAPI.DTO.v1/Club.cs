using System.ComponentModel.DataAnnotations;

using Base.Domain;


namespace PublicAPI.DTO.v1;

public class Club
{
    public Guid Id { get; set; }
    [MaxLength(64)] [MinLength(3)]
    public string Name { get; set; } = null!;
    
    public bool OwnClub { get; set; }

}