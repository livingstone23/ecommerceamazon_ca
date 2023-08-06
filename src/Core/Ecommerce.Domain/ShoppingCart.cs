using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;


//Ya tienes los elementos a punto de ser pagados
public class ShoppingCart: BaseDomainModel
{

    public Guid? ShoppingCartMasterId { get; set; }
    public virtual ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
    
    
}