using FluentValidation;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct;



public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
        public UpdateProductCommandValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .MaximumLength(50).WithMessage("{PropertyName} no debe exceder de {MaxLength} caracteres.");

            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");

            RuleFor(p => p.Stock)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");

            RuleFor(p => p.Precio)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");
                
        }

}