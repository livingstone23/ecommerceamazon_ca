using System.Linq.Expressions;
using AutoMapper;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Orders.Queries.GetOrdersById;



public class GetOrdersByIdQueryHandler : IRequestHandler<GetOrdersByIdQuery, OrderVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrdersByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    
    public async Task<OrderVm> Handle(GetOrdersByIdQuery request, CancellationToken cancellationToken)
    {

        var includes = new List<Expression<Func<Order, object>>>();
        includes.Add(x => x.OrderItems!.OrderBy(x => x.Product));
        includes.Add(x => x.OrderAddress!);


        var order = await _unitOfWork.Repository<Order>().GetEntityAsync(
            p => p.Id == request.OrderId,
            includes,
            false
        );

        return _mapper.Map<OrderVm>(order);

    }
}