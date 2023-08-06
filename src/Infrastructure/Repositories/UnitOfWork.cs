using System.Collections;
using Ecommerce.Application.Persistence;
using Ecommerce.Infrastructure.Persistence;

namespace Ecommerce.Infrastructure.Persistence.repositories;

public class UnitOfWork : IUnitOfWork
{

    //almacena en memoria todos los repositorios que se valla instanciando
    private Hashtable? _repositories;

    private readonly EcommerceDbContext _context;

    public UnitOfWork(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<int> Complete()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            
            throw new Exception("Error en la transaccion" + e.Message);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    //Hace la instanacia del repositorio sobre una entidad determinada
    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories == null) 
        {
            _repositories = new Hashtable();
        }

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type)) 
        {
            var repositoryType = typeof(RepositoryBase<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IAsyncRepository<TEntity>)_repositories[type]!;
    }
}