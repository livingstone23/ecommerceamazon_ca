

using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Products;


public class ProductForCountingSpecification: BaseSpecification<Product>
{

    public ProductForCountingSpecification(ProductSpecificationParams productParams)
        :base(x =>
        (string.IsNullOrEmpty(productParams.Search) || 
            x.Nombre!.ToLower().Contains(productParams.Search) || 
            x.Descripcion!.ToLower().Contains(productParams.Search)) &&

        (!productParams.CategoryId.HasValue || x.CategoryId == productParams.CategoryId) &&
        (!productParams.PrecioMin.HasValue || x.Precio >= productParams.PrecioMin) &&
        (!productParams.PrecioMax.HasValue || x.Precio <= productParams.PrecioMax) &&
        (!productParams.Status.HasValue || x.Status == productParams.Status) //&&
        //(!productParams.Rating.HasValue || x.Rating == productParams.Rating) &&
        //Esta seccion solo retorna el "total" de productos que cumplen con los criterios de busqueda
    )
    {
        
    }


}