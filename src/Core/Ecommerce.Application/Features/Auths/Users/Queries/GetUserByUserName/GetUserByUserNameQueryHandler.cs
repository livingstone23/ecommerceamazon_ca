using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Application.Features.Auths.Users.Vms;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByUserName;


public class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserNameQuery, AuthResponse>
{
    private readonly UserManager<Usuario> _userManager;

    public GetUserByUserNameQueryHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }


    public async Task<AuthResponse> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
    {
        
        var user = await _userManager.FindByNameAsync(request.UserName!);
        if (user is null)
        {
            throw new Exception("el usuario no existe");
        }

        var response = new AuthResponse
        {
            Id = user.Id,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Telefono = user.Telefono,
            Email = user.Email,
            UserName = user.UserName,
            Avatar = user.AvatarUrl,
            Roles = await _userManager.GetRolesAsync(user)
        };

        return response;

    }
}
