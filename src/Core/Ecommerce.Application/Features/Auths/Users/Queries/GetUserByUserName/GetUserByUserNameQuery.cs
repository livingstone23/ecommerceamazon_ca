using MediatR;
using Ecommerce.Application.Features.Auths.Users.Vms;


namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByUserName;


public class GetUserByUserNameQuery: IRequest<AuthResponse>
{
    public string? UserName { get; set; }

    public GetUserByUserNameQuery(string userName)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
    }
    
}