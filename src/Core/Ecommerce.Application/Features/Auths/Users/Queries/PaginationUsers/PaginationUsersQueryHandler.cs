


using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specifications.Users;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Queries.PaginationUsers;



public class PaginationUsersQueryHandler : IRequestHandler<PaginationUsersQuery, PaginationVm<Usuario>>
{

    private readonly IUnitOfWork _unitOfWork;

    public PaginationUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }



    public async Task<PaginationVm<Usuario>> Handle(PaginationUsersQuery request, CancellationToken cancellationToken)
    {

        var UserSpecificationParams = new UserSpecificationParams
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            Search = request.Search,
            Sort = request.Sort
        };


        //obtener los usuarios
        var spec = new UserSpecification(UserSpecificationParams);
        var users = await _unitOfWork.Repository<Usuario>().GetAllWithSpec(spec);

        //obtener el numero de usuarios
        var specCount = new UserForCountingSpecification(UserSpecificationParams);
        var totalUsers = await _unitOfWork.Repository<Usuario>().CountAsync(specCount);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalUsers) / Convert.ToDecimal(request.PageSize)); 
        var totalPages = Convert.ToInt32(rounded);

        var usersByPage = users.Count();

        var pagination = new PaginationVm<Usuario>
        {
            Count = totalUsers,
            Data = users,
            PageCount = usersByPage,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            ResultByPage = usersByPage
        };

        return pagination;

    }
}
