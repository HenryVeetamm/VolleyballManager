using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class SavedComparison : DomainEntityId
{
    public Guid ComparerId { get; set; }
    public AppUser? Comparer { get; set; }

    public Guid ComparableId { get; set; }
    public AppUser? Comparable { get; set; }
}

public class SavedComparisonDetailed : DomainEntityId
{
    public AppUserSimple Comparer { get; set; } = default!;
    
    public int TotalMatches { get; set; }
    public int Points { get; set; }
    public int Aces { get; set; }
    public int Faults { get; set; }
    public int Reception { get; set; }
}