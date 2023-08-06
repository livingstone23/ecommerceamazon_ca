


using System.Linq.Expressions;
using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCarts.Command.DeleteShoppingCartItem;


public class DeleteShoppingCartItemCommandHandler : IRequestHandler<DeleteShoppingCartItemCommand, ShoppingCartVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteShoppingCartItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    public async Task<ShoppingCartVm> Handle(DeleteShoppingCartItemCommand request, CancellationToken cancellationToken)
    {
        var shoppingCartItemToDelete = await _unitOfWork.Repository<ShoppingCartItem>().GetEntityAsync(
            p => p.Id == request.Id
        );

        if (shoppingCartItemToDelete is null)
        {
            throw new NotFoundException(nameof(ShoppingCartItem), request.Id!);
        }

        await _unitOfWork.Repository<ShoppingCartItem>().DeleteAsync(shoppingCartItemToDelete);


        var includes = new List<Expression<Func<ShoppingCart, object>>>();
        includes.Add(x => x.ShoppingCartItems!.OrderBy(x => x.Producto));
        

        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == shoppingCartItemToDelete.ShoppingCartMasterId,
            includes,
            true
        );

        return _mapper.Map<ShoppingCartVm>(shoppingCart);



        
    }
}
