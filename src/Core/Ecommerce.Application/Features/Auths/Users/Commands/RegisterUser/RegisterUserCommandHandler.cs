using MediatR;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Identity;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;

namespace Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;


public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{

    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(
                                    UserManager<Usuario> userManager, 
                                    RoleManager<IdentityRole> roleManager, 
                                    IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
        _roleManager = roleManager;
    }


    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        


        var ExisteUsuarioByEmail = await _userManager.FindByEmailAsync(request.Email!) is null ? false : true;    
        if (ExisteUsuarioByEmail)
        {
            throw new BadRequestException("El Email del usuario ya esta registrado");
        }
        
        var existerUsuarioByUserName = await  _userManager.FindByNameAsync(request.UserName!) is null ? false : true;
        if (existerUsuarioByUserName)
        {
            throw new BadRequestException("El UserName del usuario ya esta registrado");
        }

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.UserName,
            AvatarUrl = request.FotoUrl
        
        };

        var resultado = await _userManager.CreateAsync(usuario!, request.Password!);    

        if (resultado.Succeeded)
        {
            await _userManager.AddToRoleAsync(usuario, AppRole.GenericUser);
            var roles = await _userManager.GetRolesAsync(usuario);

            return new AuthResponse
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Telefono,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Avatar = usuario.AvatarUrl,
                Token = _authService.CreateToken(usuario, roles),
                Roles = roles
            };
        }

        throw new Exception("No se pudo crear el usuario");

    }

}