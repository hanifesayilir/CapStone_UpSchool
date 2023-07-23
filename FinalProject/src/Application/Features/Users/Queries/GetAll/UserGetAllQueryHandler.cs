using Application.Common.Models.General;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetAll
{
    public class UserGetAllQueryHandler : IRequestHandler<UserGetAllQuery, PaginatedList<UserGetAllDto>>
    {

        private readonly UserManager<User> _userManager;

        public UserGetAllQueryHandler(UserManager<User> userManager)
        {
            _userManager= userManager;

        }

        public async Task<PaginatedList<UserGetAllDto>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _userManager.Users.AsQueryable();

            var users = await dbQuery
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => MapToUserGetAllDto(x))
                .ToListAsync();

            return PaginatedList<UserGetAllDto>.Create(users, request.PageNumber, request.PageSize);
        }

        private static UserGetAllDto MapToUserGetAllDto(User user)
        {
            return new UserGetAllDto()
            {

                Id = user.Id,

                CreatedOn= user.CreatedOn,

                FirstName = user.FirstName,

                LastName = user.LastName,

                Email   =user.Email,

            };
        }
    }
}
