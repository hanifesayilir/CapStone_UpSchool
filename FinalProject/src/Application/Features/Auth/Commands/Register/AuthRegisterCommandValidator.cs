using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Features.Auth.Commands.Register
{
    public class AuthRegisterCommandValidator : AbstractValidator<AuthRegisterCommand>
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthRegisterCommandValidator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            RuleFor(x => x.Email)
                .MustAsync(CheckIfUserExist)
                .WithMessage("This email already exist.");
        }

        private async Task<bool> CheckIfUserExist(string email, CancellationToken cancellationToken)
        {
            var doesExist = await _authenticationService.CheckIfUserExists(email, cancellationToken);

            if (doesExist) return false;
            return true;
        }


    }
}
