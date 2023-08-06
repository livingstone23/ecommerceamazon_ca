using System.Linq.Expressions;
using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCarts.Command.UpdateShoppingCart;


public class UpdateShoppingCartCommandHandler : IRequestHandler<UpdateShoppingCartCommand, ShoppingCartVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UpdateShoppingCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }




    public async Task<ShoppingCartVm> Handle(UpdateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        
        var shoppingCartToUpdate = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            p => p.ShoppingCartMasterId == request.ShoppingCartId
        );

        if (shoppingCartToUpdate is null)
        {
            throw new NotFoundException(nameof(ShoppingCart), request.ShoppingCartId!);
        }

        var shoppingCartItems = await _unitOfWork.Repository<ShoppingCartItem>().GetAsync(
            p => p.ShoppingCartMasterId == request.ShoppingCartId
        );

        //A1-Eliminamos los anteriores items 
        _unitOfWork.Repository<ShoppingCartItem>().DeleteRange(shoppingCartItems);

        //Los objetos a insertar en la base de datos
        var shoppingCartItemsToAdd = _mapper.Map<List<ShoppingCartItem>>(request.ShoppingCartItems);

        shoppingCartItemsToAdd.ForEach(x => {
            x.ShoppingCartId = shoppingCartToUpdate.Id;
            x.ShoppingCartMasterId = request.ShoppingCartId;
        });

        //A2-Insertamos los nuevos items en la base de datos
        _unitOfWork.Repository<ShoppingCartItem>().AddRange(shoppingCartItemsToAdd);

        //A3-Actualizamos el shopping cart, permite ejecutar A1 y A2
        var resultado = await _unitOfWork.Complete();
        
        if (resultado <= 0)
        {
            throw new Exception("No se pudo agregar productos items al carrito de compra");
        }


        var includes = new List<Expression<Func<ShoppingCart, object>>>();
        includes.Add(x => x.ShoppingCartItems!.OrderBy(x => x.Producto));


        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartId,
            includes,
            true
        );

        return _mapper.Map<ShoppingCartVm>(shoppingCart);

    }

}

