using App.DAL.Contracts.Repositories;
using AutoMapper;
using AppDalDTO = App.DAL.DTO;
using AppDomain = App.Domain;
using Base.DAL.EF;
using DAL.App.EF.Mappers;

namespace DAL.App.EF.Repositories;

public class RolesInTeamRepository : BaseEntityRepository<AppDalDTO.RolesInTeam,AppDomain.RolesInTeam, AppDbContext>, IRolesInTeamRepository
{
    public RolesInTeamRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new RolesInTeamMapper(mapper))
    {
    }
}