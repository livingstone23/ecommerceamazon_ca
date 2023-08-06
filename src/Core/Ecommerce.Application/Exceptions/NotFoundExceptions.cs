

namespace Ecommerce.Application.Exceptions;


/// <summary>
/// Clase para manejar las excepciones cuando no existe un recurso
/// </summary>
public class NotFoundException: ApplicationException
{
    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) No fue encontrado.")
    {
    }
}