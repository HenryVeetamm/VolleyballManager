using AutoMapper;
using Base.BLL.Mappers;

namespace PublicAPI.DTO.v1.Mappers;

public class WorkoutTypeMapper : BaseMapper<WorkoutType , App.BLL.DTO.WorkoutType>
{
    public WorkoutTypeMapper(IMapper mapper) : base(mapper)
    {
    }

    public override WorkoutType? Map(App.BLL.DTO.WorkoutType? inObject)
    {
        return new WorkoutType()
        {
            Id = inObject!.Id,
            Description = inObject!.Description.ToString()
        };
    }
}