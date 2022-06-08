
using App.BLL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class PersonInClub : DomainEntityId, IDomainAppUserId
{
    public Guid ClubId { get; set; }
    public Club? Club { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
}