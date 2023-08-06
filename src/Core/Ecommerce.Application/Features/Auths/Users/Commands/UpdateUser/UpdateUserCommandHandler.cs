using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;


public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, AuthResponse>
{

    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;


    public UpdateUserCommandHandler(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
         
        var updateUsuario = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if(updateUsuario == null)
        {
            throw new BadRequestException($"Usuario no existe.");
        }

        updateUsuario.Nombre = request.Nombre;
        updateUsuario.Apellido = request.Apellido;
        updateUsuario.Telefono = request.Telefono;
        updateUsuario.AvatarUrl = request.FotoUrl ?? updateUsuario.AvatarUrl; //si es nulo mantenga la imagen anterior

        var resultado = await _userManager.UpdateAsync(updateUsuario);

        if(!resultado.Succeeded)
        {
            throw new Exception($"No se pudo actualizar el usuario.");
        }
        
        var userById = await _userManager.FindByEmailAsync(request.Email!);
        var roles = await _userManager.GetRolesAsync(userById!);

        var result = new AuthResponse
        {
            Id = userById!.Id,
            Nombre = userById.Nombre,
            Apellido = userById.Apellido,
            Telefono = userById.Telefono,
            Email = userById.Email,
            UserName = userById.UserName,
            Avatar = userById.AvatarUrl,
            Token = _authService.CreateToken(userById, roles),
            Roles = roles
        };

        return result;

    }

}
