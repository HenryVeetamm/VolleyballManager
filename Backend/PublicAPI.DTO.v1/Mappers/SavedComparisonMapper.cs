using AutoMapper;
using Base.BLL.Mappers;
using PublicAPI.DTO.v1.Identity;

namespace PublicAPI.DTO.v1.Mappers;

public class SavedComparisonMapper : BaseMapper<SavedComparison, App.BLL.DTO.SavedComparison>
{
    public SavedComparisonMapper(IMapper mapper) : base(mapper)
    {
    }

    public SavedComparisonDetailed MapDetailed(App.BLL.DTO.SavedComparisonDetailed inObject)
    {
        return new SavedComparisonDetailed()
        {
            Comparer = new AppUserSimple()
            {
                FirstName = inObject!.Comparer.FirstName,
                LastName = inObject!.Comparer.LastName,
            },
            Aces = inObject!.Aces,
            Faults = inObject!.Faults,
            TotalMatches = inObject!.TotalMatches,
            Points = inObject!.Points,
            Reception = inObject!.Reception
        };
    }
}