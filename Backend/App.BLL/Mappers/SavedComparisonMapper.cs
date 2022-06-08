using AutoMapper;
using Base.BLL.Mappers;
using AppDalDTO = App.DAL.DTO;
using AppBllDto = App.BLL.DTO;
namespace App.BLL.Mappers;

public class SavedComparisonMapper : BaseMapper<AppBllDto.SavedComparison, AppDalDTO.SavedComparison>
{
    public SavedComparisonMapper(IMapper mapper) : base(mapper)
    {
    }
}