using Ecommerce.Application.Features.Orders.Vms;
using MediatR;

namespace Ecommerce.Application.Features.Payments.Commands.CreatePayment;

public class CreatePaymentCommand : IRequest<OrderVm>
{


    //Indicamos la orden a pagar
    public int OrderId {get;set;}

    //Indicamos el carrito de compras a eliminar una vez realizado el pago
    public Guid? ShoppingCartMasterId {get;set;}    
    
}


