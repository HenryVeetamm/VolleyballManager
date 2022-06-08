using Base.Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseUOW<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    protected readonly TDbContext UOWDbContext;

    private readonly Dictionary<Type, object> _repos = new();

    public BaseUOW(TDbContext dbContext)
    {
        UOWDbContext = dbContext;
    }
    public async Task<int> SaveChangesAsync()
    {
        return await UOWDbContext.SaveChangesAsync();
    }

    public int SaveChanges()
    {
        return UOWDbContext.SaveChanges();
    }

    public TRepository GetRepository<TRepository>(Func<TRepository> repoCreate) where TRepository : class
    {
        if (_repos.TryGetValue(typeof(TRepository), out var repo)) return (TRepository)repo;

        var repoInstance = repoCreate();
        _repos.Add(typeof(TRepository), repoInstance);

        return repoInstance;
    }
}