using MediatR;
using Ecommerce.Domain;
using Ecommerce.Application.Features.Shared.Queries;



namespace Ecommerce.Application.Features.Auths.Users.Queries.PaginationUsers;

public class PaginationUsersQuery: PaginationBaseQuery, IRequest<PaginationVm<Usuario>>
{
    



}
