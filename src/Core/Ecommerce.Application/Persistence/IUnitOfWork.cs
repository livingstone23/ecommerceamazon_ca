


namespace Ecommerce.Application.Persistence;


public interface IUnitOfWork: IDisposable 
{
    //hace referencia a la instancia de un repositorio
    IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class;

    //Metodo de confirmacion que indica la culminacion de tarea.
    Task<int> Complete();

}