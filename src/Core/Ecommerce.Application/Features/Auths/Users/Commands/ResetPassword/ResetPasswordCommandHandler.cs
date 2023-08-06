using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPassword;



public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{

    private readonly UserManager<Usuario> _userManager;
    private readonly IAuthService _authService;

    public ResetPasswordCommandHandler(UserManager<Usuario> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }



    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var updateUsuario = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if (updateUsuario is null)
        {
            throw new BadRequestException("El usuario no existe.");
        }

        //Permite validar que el password es valido
        var resultValidateOldPassword = _userManager.PasswordHasher.VerifyHashedPassword(updateUsuario, updateUsuario.PasswordHash!, request.OldPassword!);

        if(!resultValidateOldPassword.Equals(PasswordVerificationResult.Success))
        {
            throw new BadRequestException("El password ingresado no es valido.");
        }

        var hashedNewPassword = _userManager.PasswordHasher.HashPassword(updateUsuario, request.NewPassword!);
        updateUsuario.PasswordHash = hashedNewPassword;

        var result = await _userManager.UpdateAsync(updateUsuario);

        if(!result.Succeeded)
        {
            throw new Exception("No se pudo actualizar el password.");
        }
        

        return Unit.Value;

    }
}
