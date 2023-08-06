using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminStatusUser;


public class UpdateAdminStatusUserCommandHandler: IRequestHandler<UpdateAdminStatusUserCommand, Usuario>
{
    private readonly UserManager<Usuario> _userManager;

    public UpdateAdminStatusUserCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Usuario> Handle(UpdateAdminStatusUserCommand request, CancellationToken cancellationToken)
    {
        var updateUsuario = await _userManager.FindByIdAsync(request.Id!);
        if (updateUsuario == null)
        {
            throw new BadRequestException($"El Usuario no existe.");
        }

        updateUsuario.IsActive = !updateUsuario.IsActive;
        var result = await _userManager.UpdateAsync(updateUsuario);

        if(!result.Succeeded)
        {
            throw new Exception($"No se pudo actualizar el usuario.");
        }

        return updateUsuario;

    }

}