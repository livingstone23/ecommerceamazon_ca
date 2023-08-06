


using AutoMapper;
using Ecommerce.Application.Features.Countries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Countries.Queries.GetCountryList;


public class GetCountryListQueryHandler : IRequestHandler<GetCountryListQuery, IReadOnlyList<CountryVm>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    
    public GetCountryListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<IReadOnlyList<CountryVm>> Handle(GetCountryListQuery request, CancellationToken cancellationToken)
    {
        var Countries = await _unitOfWork.Repository<Country>().GetAsync(
            null,                               //no quiero filtros
            x => x.OrderBy(y => y.Name),        //ordenamiento
            string.Empty,                       //sin relaciones con otras entidades
            false
        );

        var resultado =  _mapper.Map<IReadOnlyList<CountryVm>>(Countries);
        return resultado;
        
    }



}