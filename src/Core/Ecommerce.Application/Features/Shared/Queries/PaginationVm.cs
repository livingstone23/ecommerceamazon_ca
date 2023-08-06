

namespace Ecommerce.Application.Features.Shared.Queries;

//Resultado que representa los resultados de la paginacion que se enviaran al cliente
public class PaginationVm<T> where T : class
{
    public int Count { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public IReadOnlyList<T>? Data { get; set; }

    //public int PageCount => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

    public int PageCount { get; set; }

    public int ResultByPage {get; set;}
}