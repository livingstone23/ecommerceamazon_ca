



namespace Ecommerce.Application.Features.Products.Commands.CreateProduct;

//Representa el conjunto de imagenes a manejar por producto.
public class CreateProductImageCommand
{
    public string? Url { get; set; }

    public int ProductId { get; set; }

    public string? PublicCode { get; set; }

}