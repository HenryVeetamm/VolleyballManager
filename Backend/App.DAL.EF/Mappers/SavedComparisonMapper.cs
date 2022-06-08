using AutoMapper;
using Base.DAL.EF.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
namespace DAL.App.EF.Mappers;

public class SavedComparisonMapper : BaseMapper<AppDalDTO.SavedComparison, AppDomain.SavedComparison>
{
    public SavedComparisonMapper(IMapper mapper) : base(mapper)
    {
    }
}