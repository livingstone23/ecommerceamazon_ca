


using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser;


public class UpdateAdminUserCommandHandler: IRequestHandler<UpdateAdminUserCommand, Usuario>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authServices;

    public UpdateAdminUserCommandHandler(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IAuthService authServices)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authServices = authServices;
    }

    

    public async Task<Usuario> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
    {
        var updateUsuario = await _userManager.FindByIdAsync(request.Id!);
        if (updateUsuario == null)
        {
            throw new BadRequestException("El usuario no existe");
        }

        updateUsuario.Nombre = request.Nombre;
        updateUsuario.Apellido = request.Apellido;
        updateUsuario.Telefono = request.Telefono;

        //Actualizamos los datos del usuario
        var result = await _userManager.UpdateAsync(updateUsuario);


        if (!result.Succeeded)
        {
            throw new Exception("No se pudo actualizar el usuario");
        }

        var role = await _roleManager.FindByNameAsync(request.Role!);
        if (role is null)
        {
            throw new BadRequestException("El rol asignado no existe");
        }

        await _userManager.AddToRoleAsync(updateUsuario, role.Name!);

        return updateUsuario;

    }
}