using Base.Domain;
using PublicAPI.DTO.v1.Identity;
using AppUser = PublicAPI.DTO.v1.Identity.AppUser;

namespace PublicAPI.DTO.v1;

public class SavedComparison
{
    public Guid Id { get; set; }
    public Guid ComparerId { get; set; }
    public AppUser? Comparer { get; set; }

    public Guid ComparableId { get; set; }
    public AppUser? Comparable { get; set; }
}

public class SavedComparisonDetailed
{
public AppUserSimple Comparer { get; set; } = default!;
public int TotalMatches { get; set; }
public int Points { get; set; }
public int Aces { get; set; }
public int Faults { get; set; }
public int Reception { get; set; }
}