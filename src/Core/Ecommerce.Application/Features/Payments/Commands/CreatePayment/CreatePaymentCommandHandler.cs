



using AutoMapper;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.Extensions.Options;

namespace Ecommerce.Application.Features.Payments.Commands.CreatePayment;



public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, OrderVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public CreatePaymentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }




    public async Task<OrderVm> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        
        var orderToPay = await _unitOfWork.Repository<Order>().GetEntityAsync(
            x => x.Id == request.OrderId,
            null,
            false
        );

        //Actualizamos el estados
        orderToPay.Status = OrderStatus.Completed;
        //Actualizamos el estado del objeto
        _unitOfWork.Repository<Order>().UpdateEntity(orderToPay);

        //Obtenemos los elementos a eliminar del carrito de compras
        var ShoppingCartItems = await _unitOfWork.Repository<ShoppingCartItem>().GetAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartMasterId
        );

        //Eliminamos los elementos del carrito de compras
        _unitOfWork.Repository<ShoppingCartItem>().DeleteRange(ShoppingCartItems);

        //Para confirmar la transaccion
        await _unitOfWork.Complete();


        return _mapper.Map<OrderVm>(orderToPay);

    }




}
    

   