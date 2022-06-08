namespace Base.Contracts.DAL;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(); 
    int SaveChanges();

    TRepository GetRepository<TRepository>(Func<TRepository> repoCreate) where TRepository : class;
}