

using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.Login
{
    public class AuthLoginCommandHandler : IRequestHandler<AuthLoginCommand, AuthLoginDto>
    {
        private readonly IAuthenticationService _authentificationService;

        public AuthLoginCommandHandler(IAuthenticationService authenticationService)
        {
            _authentificationService = authenticationService;
        }

        public async Task<AuthLoginDto> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
        {
            var jwtDto = await _authentificationService.LoginAsync(MapLoginCommandToRequest(request), cancellationToken);

            return MapJwtDtoToAuthLoginDto(jwtDto);
        }

        private AuthLoginDto MapJwtDtoToAuthLoginDto(JwtDto jwt) => new AuthLoginDto(jwt.AccessToken, jwt.ExpiryDate);

        private AuthLoginRequest MapLoginCommandToRequest(AuthLoginCommand command) => new AuthLoginRequest(command.Email, command.Password);
    }
}
