


using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Users;


//colocara la logica de los parametros de la busqueda.
public class UserSpecification: BaseSpecification<Usuario>
{

    public UserSpecification(UserSpecificationParams userParams): 
        base(
            x =>
                (string.IsNullOrEmpty(userParams.Search) || x.Nombre!.Contains(userParams.Search) ||
                string.IsNullOrEmpty(userParams.Search) || x.Apellido!.Contains(userParams.Search)
            )
        )
    {
        //Logica de ordenamiento.
        //Desde donde comenzaremos a buscar a contar los elementos que devolvera
        ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1), 
        //Cuantos elementos vamos a tomar
        userParams.PageSize);    
        
        //Logica de ordenamiento.
        if(!string.IsNullOrEmpty(userParams.Sort))
        {
            switch(userParams.Sort)
            {
                case "nombreAsc":
                    AddOrderBy(x => x.Nombre!);
                    break;
                case "nombreDesc":
                    AddOrderByDescending(x => x.Nombre!);
                    break;

                case "apellidoAsc":
                    AddOrderBy(x => x.Apellido!);
                    break;

                case "apellidoDesc":
                    AddOrderByDescending(x => x.Apellido!);
                    break;

                default:
                    AddOrderBy(x => x.Nombre!);
                    break;
            }
        }
        else 
        {
            AddOrderByDescending(x => x.Nombre!);
        }
        
    }
}
