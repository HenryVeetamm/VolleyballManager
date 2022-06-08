using App.DAL.Contracts.Repositories;
using AppBllDTO = App.BLL.DTO;
using AppDalDTO = App.DAL.DTO;
using Base.Contracts.BLL.Services;

namespace App.BLL.Contracts.Services;

public interface IUserService : IBaseEntityService<AppBllDTO.Identity.AppUser, AppDalDTO.Identity.AppUser>,
    IUserRepositoryCustom<AppBllDTO.Identity.AppUser>
{
    Task<IEnumerable<AppBllDTO.Identity.AppUser>> GetAllClubMembers(Guid userId, bool noTracking = true);
    
}