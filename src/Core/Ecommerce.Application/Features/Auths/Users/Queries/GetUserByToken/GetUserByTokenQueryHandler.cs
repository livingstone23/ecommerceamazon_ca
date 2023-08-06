

using MediatR;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Persistence;
using AutoMapper;
using Ecommerce.Application.Features.Addresses.Vms;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByToken;


public class GetUserByTokenQueryHandler : IRequestHandler<GetUserByTokenQuery, AuthResponse>
{

    private readonly UserManager<Usuario> _userManager;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByTokenQueryHandler(UserManager<Usuario> userManager, IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userManager = userManager;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    public async Task<AuthResponse> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
    {

        var user = await _userManager.FindByNameAsync(_authService.GetSessionUser());

        if (user is null)
        {
            throw new Exception("el usuario no esta autenticado");
        }

        if(!user.IsActive)
        {
            throw new Exception("el usuario esta bloqueado, contacte con el admin");
        }

        var direccionEnvio = await _unitOfWork.Repository<Address>().GetEntityAsync(
                x =>x.UserName == user.UserName
            );

        var roles = await _userManager.GetRolesAsync(user);

        var result = new AuthResponse
        {
            Id = user.Id,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Telefono = user.Telefono,
            Email = user.Email,
            UserName = user.UserName,
            Avatar = user.AvatarUrl,
            DireccionEnvio = _mapper.Map<AddressVm>(direccionEnvio),
            Token = _authService.CreateToken(user, roles),
            Roles = roles
        };

        return result;

    }


}








