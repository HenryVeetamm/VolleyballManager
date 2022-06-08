using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.BLL.DTO.Enums;
using Base.Domain;
using Base.Domain.Identity;

namespace App.BLL.DTO.Identity;

public class AppUser: BaseUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Birthday { get; set; }

    public EGender Gender { get; set; }

    [MaxLength(16)] [MinLength(3)]
    public string NationalCode { get; set; } = null!;

    public ICollection<PersonInWorkout>? PersonInWorkouts { get; set; }
    
    [InverseProperty(nameof(SavedComparison.Comparer))]
    public ICollection<SavedComparison>? Comparers { get; set; }

    [InverseProperty(nameof(SavedComparison.Comparable))]
    public ICollection<SavedComparison>? Comparables { get; set; }

    public ICollection<PersonInClub>? PersonInClubs { get; set; }

    public ICollection<PersonInTeam>? PersonInTeams { get; set; }

    public ICollection<PersonInMatch>? PersonInMatches { get; set; }

    public ICollection<Announcement>? Announcements { get; set; }

    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}

public class AppUserSimple : DomainEntityId
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}