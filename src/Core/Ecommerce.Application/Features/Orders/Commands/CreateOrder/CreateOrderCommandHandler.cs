using System.Linq.Expressions;
using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Stripe;

namespace Ecommerce.Application.Features.Orders.Commands.CreateOrder;


public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly UserManager<Usuario> _userManager;
    private readonly StripeSettings _stripeSettings;
    
    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, 
                                    IMapper mapper, 
                                    IAuthService authService,
                                     UserManager<Usuario> userManager, 
                                     IOptions<StripeSettings> stripeSettings)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authService = authService;
        _userManager = userManager;
        _stripeSettings = stripeSettings.Value;
    }


    public async Task<OrderVm> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        
        //Eliminamos ordenes pendientes del usuario.
        var orderPending = await _unitOfWork.Repository<Order>().GetEntityAsync(
            o => o.CompradorUserName == _authService.GetSessionUser() && o.Status == OrderStatus.Pending,
            null,                       //No es requerido objetos adjuntos
            true                        //que la mantenga en memori
        );

        //Si existe una orden pendiente, la eliminamos
        if (orderPending is not null)
        {
           await _unitOfWork.Repository<Order>().DeleteAsync(orderPending);
        }

        //Crearemos la orden de los datos del  shopping cart y sus elementos.
        var includes = new List<Expression<Func<ShoppingCart, object>>>();
        includes.Add(x => x.ShoppingCartItems!.OrderBy(x => x.Producto));

        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartId,
            includes,
            false
        );

        //Obtenemos la direccion del usuario
        //1ro obtenemos el usuario
        var user = await _userManager.FindByNameAsync(_authService.GetSessionUser());

        if(user is null)
        {
            throw new Exception("El usuario no esta autenticado");
        }

        //2do obtenemos la direccion del usuario
        var direction = await _unitOfWork.Repository<Ecommerce.Domain.Address>().GetEntityAsync(
            x => x.UserName == user.UserName,
            null,
            false
        );

        //Creamos una "OrderAddress" para la orden
        OrderAddress orderAddress = new()
        {
            Direccion = direction.Direccion,
            Ciudad = direction.Ciudad,
            CodigoPostal = direction.CodigoPostal,
            Pais = direction.Pais,
            Departamento = direction.Departamento,
            UserName = direction.UserName
        };

        //Insertamos el "OrderAddress" en la base de datos
        await _unitOfWork.Repository<OrderAddress>().AddAsync(orderAddress);

        //Obtenemos los datos calculados para la orden de compra.
        var subtotal = Math.Round(shoppingCart.ShoppingCartItems!.Sum(x => x.Precio * x.Cantidad),2);
        var impuesto = Math.Round(subtotal *Convert.ToDecimal(0.18),2);
        var precioEnvio = subtotal < 100 ? 10 : 25;
        var total = subtotal + impuesto + precioEnvio;

        //Preparamos los datos del comprador
        var nombreComprador = $"{user.Nombre} {user.Apellido}";

        //Creamos la orden de compra
        var order = new Order(nombreComprador, user.UserName!, orderAddress, subtotal, total, impuesto, precioEnvio);

        //Insertamos la orden de compra en la base de datos
        //1ro insertamos la cabecera de la orden
        await _unitOfWork.Repository<Order>().AddAsync(order);

        //2do insertamos los detalles de la orden
        var items = new List<OrderItem>();

        foreach (var shoppingElement in shoppingCart.ShoppingCartItems!)
        {
            var orderItem = new OrderItem
            {
                ProductNombre = shoppingElement.Producto,
                ProductId = shoppingElement.ProductId,
                ImagenUrl = shoppingElement.Imagen,
                Precio = shoppingElement.Precio,
                Cantidad = shoppingElement.Cantidad,
                OrderId = order.Id
            };

            items.Add(orderItem);
        }

        //Insertamos los detalles de la orden en la base de datos
        _unitOfWork.Repository<OrderItem>().AddRange(items);

        //Guardamos la transaccion
        var resultado = await _unitOfWork.Complete();

        if(resultado <=0 ) 
        {
            throw new Exception("Error creando la orden de compra.");
        }

        //Creamos la comunicacion con el servicio del stripe
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        var service = new PaymentIntentService();
        PaymentIntent intent;

        //Si no existe la orden de compra un intent
        if(string.IsNullOrEmpty(order.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount =    (long)(order.Total * 100),               //Convertimos a formato long
                Currency = "usd",                                   //Por defecto moneda dolar
                PaymentMethodTypes = new List<string> {"card"}      //Tipo de pago tarjeta.
            };

            intent = await service.CreateAsync(options);
            order.PaymentIntentId = intent.Id;
            order.ClientSecret = intent.ClientSecret;
            order.StripeApiKey = _stripeSettings.Publishblekey;
        } 
        else    //Para una orden de compra ya existente, actualizamos su total
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)order.Total
            };

            await service.UpdateAsync(order.PaymentIntentId, options);

        }

        //Actualizamos la orden de compra
        _unitOfWork.Repository<Order>().UpdateEntity(order);
        var resultadoOrder = await _unitOfWork.Complete();

        if(resultadoOrder <= 0)
        {
            throw new Exception("Error creando el Payment intent en stripe.");
        }


        return _mapper.Map<OrderVm>(order);
    }
}