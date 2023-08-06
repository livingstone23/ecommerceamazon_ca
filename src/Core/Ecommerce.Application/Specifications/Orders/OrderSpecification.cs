using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Orders;

public class OrderSpecification : BaseSpecification<Order>
{
     public OrderSpecification(OrderSpecificationParams orderParams)
           : base(
                   x =>
                    (
                        string.IsNullOrEmpty(orderParams.Username) || 
                            x.CompradorUserName!.Contains(orderParams.Username)) &&
                        (!orderParams.Id.HasValue || x.Id == orderParams.Id) 
                 )
        {
            //Objetos a agregar en el respuesta.
            AddInclude(p => p.OrderItems!);  

            //Aplicacion de paginacion
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);

            //Evaluamos si envia parametro de ordenamiento
             if (!string.IsNullOrEmpty(orderParams.Sort))
             {

                switch (orderParams.Sort)
                {
                    case "createDateAsc":
                        AddOrderBy(p => p.CreatedDate!);
                        break;

                    case "createDateDesc":
                        AddOrderByDescending(p => p.CreatedDate!);
                        break;
 
                    default:
                        AddOrderBy(p => p.CreatedDate!);
                        break;
                }
             }
             else
             {
                AddOrderByDescending(p => p.CreatedDate!);
             }

        }


}