using Ecommerce.Application.Features.ShoppingCarts.Vms;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCarts.Command.UpdateShoppingCart;

public class UpdateShoppingCartCommand : IRequest<ShoppingCartVm>
{
    public Guid ShoppingCartId { get; set; }
    public List<ShoppingCartItemVm>? ShoppingCartItems { get; set; }
}







