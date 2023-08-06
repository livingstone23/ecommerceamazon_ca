using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;


public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("{PropertyName} es requerido")
            .NotNull()
            .EmailAddress().WithMessage("{PropertyName} no es valido");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
            .NotNull()
            .MinimumLength(12).WithMessage("{PropertyName} debe tener al menos 12 caracteres");
    }
}