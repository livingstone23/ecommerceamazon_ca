using FluentValidation;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct;



//Manejo de validacion para crear el producto.
public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("{PropertyName} es requerido")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} no debe exceder los {MaxLength} caracteres");

        RuleFor(p => p.Descripcion)
            .NotEmpty().WithMessage("{PropertyName} es requerido")
            .NotNull()
            .MaximumLength(4000).WithMessage("{PropertyName} no debe exceder los {MaxLength} caracteres");
    
        RuleFor(p => p.Stock)
            .NotEmpty().WithMessage("{PropertyName} es requerido")
            .NotNull();
            
        RuleFor(p => p.Precio)
            .NotEmpty().WithMessage("{PropertyName} es requerido")
            .NotNull();
    }


}