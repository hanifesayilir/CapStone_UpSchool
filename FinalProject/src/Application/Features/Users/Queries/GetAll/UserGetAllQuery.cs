using Application.Common.Models.General;
using MediatR;

namespace Application.Features.Users.Queries.GetAll
{
    public class UserGetAllQuery: IRequest<PaginatedList<UserGetAllDto>>
    {

        public int PageNumber { get; set; }

        public int PageSize { get; set;}

        public UserGetAllQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;    
        }
    }
}
