namespace Base.Contracts.BLL;

public interface IBaseBLL
{
    Task<int> SaveChangesAsync(); 
    
    int SaveChanges();

    TService GetService<TService>(Func<TService> serviceCreate) where TService : class;
}