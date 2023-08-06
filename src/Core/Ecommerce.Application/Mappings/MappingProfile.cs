using AutoMapper;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Categories.Vms;
using Ecommerce.Application.Features.Countries.Vms;
using Ecommerce.Application.Features.Images.Queries.Vms;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Reviews.Queries.Vms;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Domain;

namespace Ecommerce.Application.Mappings;

public class MappingProfile: Profile 
{

    public MappingProfile()
    {
        
        CreateMap<Product, ProductVm>()
            .ForMember(d => d.CategoryNombre, opt => opt.MapFrom(s => s.Category!.Nombre))
            .ForMember(d => d.NumeroReviews, x => x.MapFrom(s => s.Reviews == null ? 0 : s.Reviews.Count));
        CreateMap<CreateProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>();
        CreateMap<CreateProductImageCommand, Image>();


        CreateMap<Image, ImageVm>();


        CreateMap<Review, ReviewVm>();


        CreateMap<Country, CountryVm>();


        CreateMap<Category, CategoryVm>();


        CreateMap<ShoppingCart, ShoppingCartVm>()
            .ForMember(p => p.ShoppingCartId, x => x.MapFrom(a => a.ShoppingCartMasterId));
            
        CreateMap<ShoppingCartItem, ShoppingCartItemVm>();
        CreateMap<ShoppingCartItemVm, ShoppingCartItem>();

        CreateMap<Address, AddressVm>();


        CreateMap<Order, OrderVm>();
        CreateMap<OrderItem, OrderItemVm>();
        CreateMap<OrderAddress, AddressVm>();

    }



}
