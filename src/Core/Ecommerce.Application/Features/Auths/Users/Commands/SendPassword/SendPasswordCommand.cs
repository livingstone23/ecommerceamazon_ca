using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Commands.SendPassword;

public class SendPasswordCommand : IRequest<string>
{
    //Propiedada a devolver por el cliente.
    public string? Email { get; set; }

}












