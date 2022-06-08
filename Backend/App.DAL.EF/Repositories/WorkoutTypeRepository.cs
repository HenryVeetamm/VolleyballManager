using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;

namespace DAL.App.EF.Repositories;

public class WorkoutTypeRepository : BaseEntityRepository<AppDalDTO.WorkoutType,AppDomain.WorkoutType, AppDbContext>, IWorkoutTypeRepository
{
    public WorkoutTypeRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new WorkoutTypeMapper(mapper))
    {
    }
}