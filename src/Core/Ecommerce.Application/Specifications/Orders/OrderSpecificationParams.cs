using Ecommerce.Application.Specifications;


namespace Ecommerce.Application.Specifications.Orders;


//1- Para el proceso de paginacion, es la primera clase a diseñar
public class OrderSpecificationParams : SpecificationParams
{
    public string? Username { get; set; }
    public int? Id { get; set; } 

}