using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Domain.Configuration;



public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        //Implementamos la relacion 1 a 1 entre Order y OrderAddress
        builder.OwnsOne(o => o.OrderAddress, x =>
        {
            x.WithOwner();
        });

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        //se almacena el valor del status en la base de datos como un string
        builder.Property(s => s.Status).HasConversion(
            o => o.ToString(),
            o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
            );

        
    }
}