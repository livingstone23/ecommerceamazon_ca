using MediatR;
using System.Text;
using Ecommerce.Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Domain;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Models.Email;

namespace Ecommerce.Application.Features.Auths.Users.Commands.SendPassword;



public class SendPasswordCommandCommandHandler : IRequestHandler<SendPasswordCommand, string>
{

    private readonly IEmailServices _emailService;
    private readonly UserManager<Usuario> _userManager;

    public SendPasswordCommandCommandHandler(IEmailServices emailService, UserManager<Usuario> userManager)
    {
        _emailService = emailService;
        _userManager = userManager;
    }


    public async Task<string> Handle(SendPasswordCommand request, CancellationToken cancellationToken)
    {
        
        var usuario = await _userManager.FindByEmailAsync(request.Email!);
        if (usuario is null)
        {
            throw new BadRequestException($"Usuario con email {request.Email} no encontrado");
        }

        //Permite crear el token para agregar en el correo a enviar
        var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
        var plainTextBytes = Encoding.UTF8.GetBytes(token);
        token = Convert.ToBase64String(plainTextBytes);

        var emailMessage = new EmailMessage
        {
            To =  request.Email,
            Subject = "Cambiar contraseña",
            Body = $"Para cambiar tu contraseña haga click :"
        };

        var result = await _emailService.SendEmail(emailMessage, token);

        if(!result)
        {
            throw new Exception("No se pudo enviar el Email");
        }

        return $"Se envio un correo a {request.Email} para recuperar su contraseña";

    }
}