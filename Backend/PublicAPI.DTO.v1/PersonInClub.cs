
using Base.Contracts.Domain;
using Base.Domain;
using PublicAPI.DTO.v1.Identity;

namespace PublicAPI.DTO.v1;

public class PersonInClub
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Club? Club { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
}