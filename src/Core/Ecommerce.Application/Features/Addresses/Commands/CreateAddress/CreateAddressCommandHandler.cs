using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Addresses.Commands.CreateAddress;


public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, AddressVm>
{

    private readonly IAuthService _authService;         //por ser un usuario que esta en sesion
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public CreateAddressCommandHandler(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    public async Task<AddressVm> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        //Buscamos si el usuario ya tiene una direccion
        var addressRecord = await _unitOfWork.Repository<Address>().GetEntityAsync(
            x => x.UserName == _authService.GetSessionUser(),   //Condicion de busqueda.
            null,                                               //Obtener otras entidades
            false
        );

        if(addressRecord is null)
        {
            //si no tiene direccion
            addressRecord = new Address
            {
                Direccion = request.Direccion,
                Ciudad = request.Ciudad,
                Departamento = request.Departamento,
                CodigoPostal = request.CodigoPostal,
                Pais = request.Pais,
                UserName = _authService.GetSessionUser()
            };
            _unitOfWork.Repository<Address>().AddEntity(addressRecord);
        }
        else
        {
            //si ya tiene direccion, actualizamos el dato.
            addressRecord.Direccion = request.Direccion;
            addressRecord.Ciudad = request.Ciudad;
            addressRecord.Departamento = request.Departamento;
            addressRecord.CodigoPostal = request.CodigoPostal;
            addressRecord.Pais = request.Pais;
       
        }

        await _unitOfWork.Complete();


        return _mapper.Map<AddressVm>(addressRecord);


    }

}


