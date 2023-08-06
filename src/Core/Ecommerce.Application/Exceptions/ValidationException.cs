

using FluentValidation.Results;

namespace Ecommerce.Application.Exceptions;


/// <summary>
/// Clase para manejar las excepciones de validacion
/// </summary>
public class ValidationException: ApplicationException 
{

    public IDictionary<string, string[]> Errors { get; }

    //si no envio las validaciones el se encarga de crear el diccionario
    public ValidationException(): base("Se presentaron uno o mas errores de validacion")
    {
        Errors = new Dictionary<string, string[]>();
    }

    //Agrupa las validaciones por propiedad si se las envio
    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }


}