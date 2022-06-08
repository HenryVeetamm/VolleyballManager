using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace Base.BLL;

public class BaseBLL<TUnitOfWork> : IBaseBLL
where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork Uow;

    private readonly Dictionary<Type, object> _serviceCache = new();

    public BaseBLL(TUnitOfWork uow)
    {
        Uow = uow;
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await Uow.SaveChangesAsync();
    }

    public int SaveChanges()
    {
        return Uow.SaveChanges();
    }

    public TService GetService<TService>(Func<TService> serviceCreate) where TService : class
    {
        if (_serviceCache.TryGetValue(typeof(TService), out var service)) return (TService) service;

        var serviceInstance = serviceCreate();
        
        _serviceCache.Add(typeof(TService), serviceInstance);

        return serviceInstance;
    }
}