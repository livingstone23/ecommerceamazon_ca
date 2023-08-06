using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Users;

//Para conocer el numero a obtener.
public class UserForCountingSpecification : BaseSpecification<Usuario>
{
    public UserForCountingSpecification(UserSpecificationParams userParams): 
        base(
            x =>
                (string.IsNullOrEmpty(userParams.Search) || x.Nombre!.Contains(userParams.Search) ||
                string.IsNullOrEmpty(userParams.Search) || x.Apellido!.Contains(userParams.Search)
            )
        )
    {
  
    }
}