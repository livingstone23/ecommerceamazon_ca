using Ecommerce.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain;

//Ya cuenta con la informacion de strype
public class Order: BaseDomainModel
{
    public Order() {} 

    public Order(
        string compradorNombre,
        string compradorEmail,
        OrderAddress orderAddress,
        decimal subtotal,
        decimal total,
        decimal impuesto,
        decimal precioEnvio
    ) {
        CompradorNombre = compradorNombre;
        CompradorUserName = compradorEmail;
        OrderAddress = orderAddress;
        Subtotal = subtotal;
        Total = total;
        Impuesto = impuesto;
        PrecioEnvio = precioEnvio;

    }


    public string? CompradorNombre { get; set; }

    public string? CompradorUserName { get; set; }

    public OrderAddress? OrderAddress { get; set; }

    public IReadOnlyList<OrderItem>? OrderItems { get; set; }

    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal Subtotal { get; set; }
    
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal Total { get; set; }

    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal Impuesto { get; set; }

    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal PrecioEnvio { get; set; }

    public string? PaymentIntentId { get; set; }

    public string? ClientSecret { get; set; }

    public string? StripeApiKey { get; set; }

}