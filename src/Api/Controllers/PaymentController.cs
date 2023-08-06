using System.Net;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Features.Payments.Commands.CreatePayment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaymentController: ControllerBase 
{

    private IMediator _mediator;
    private readonly IAuthService _authService;

    public PaymentController(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }


    [HttpPost(Name = "CreatePayment")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderVm>>  CreatePayment([FromBody]  CreatePaymentCommand request)
    {
        return await _mediator.Send(request);
    }




}