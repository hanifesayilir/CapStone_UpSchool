using Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Login
{
    public class AuthLoginCommandValidator :AbstractValidator<AuthLoginCommand>
    {
        private readonly IAuthenticationService _authentificationService;

        public AuthLoginCommandValidator(IAuthenticationService authenticationService)
        {

            _authentificationService = authenticationService;

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Your Email or password is incorrect.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Your Email or password is incorrect.");

            RuleFor(x => x.Email)
               .MustAsync(CheckIfUserExists)
               .WithMessage("Your Email or password is incorrect.");

        }

        private Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken)
        {
            return _authentificationService.CheckIfUserExists(email, cancellationToken);
        }
    }
}
