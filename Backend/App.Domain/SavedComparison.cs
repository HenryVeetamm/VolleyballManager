using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(ComparerId), nameof(ComparableId), IsUnique = true)]
public class SavedComparison : DomainEntityId
{
    public Guid ComparerId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.SavedComparison), Name = nameof(Comparer))]
    public AppUser? Comparer { get; set; }

    public Guid ComparableId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.SavedComparison), Name = nameof(Comparable))]
    public AppUser? Comparable { get; set; }
}