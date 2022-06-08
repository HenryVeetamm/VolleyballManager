using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using Base.Domain;

namespace App.DAL.DTO;

public class SavedComparison : DomainEntityId
{
    public Guid ComparerId { get; set; }
    public AppUser? Comparer { get; set; }

    public Guid ComparableId { get; set; }
    public AppUser? Comparable { get; set; }
}