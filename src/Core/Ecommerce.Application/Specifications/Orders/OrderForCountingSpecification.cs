
using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Orders;


//2- Para el proceso de paginacion, es la segunda clase a diseñar
public class OrderForCountingSpecification : BaseSpecification<Order>
{
    
    //Constructor almacena la logica de la consulta
     public OrderForCountingSpecification(OrderSpecificationParams orderParams)
           : base(
                   x =>
                    (
                        string.IsNullOrEmpty(orderParams.Username) || 
                        x.CompradorUserName!.Contains(orderParams.Username)) &&
                        (!orderParams.Id.HasValue || x.Id == orderParams.Id) 
                 )
        {
        }
}