using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.Email;
using Application.Features.Products.Queries.GetAll;
using Domain.Entities;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Register
{
    public class AuthRegisterCommandHandler : IRequestHandler<AuthRegisterCommand, AuthRegisterDto>
    {

        private IAuthenticationService _authentificationService;

        private readonly IJwtService _jwtService;

        public readonly IEmailService _emailService;

        private readonly IApplicationDbContext _applicationDbContext;
        public AuthRegisterCommandHandler(IAuthenticationService authentificationService, IJwtService jwtService, IEmailService emailService, IApplicationDbContext applicationDbContext)
        {
            _authentificationService = authentificationService;
            _jwtService = jwtService;
            _emailService = emailService;
            _applicationDbContext = applicationDbContext;
           
        }

        public async Task<AuthRegisterDto> Handle(AuthRegisterCommand request, CancellationToken cancellationToken)
        {
            var createUserDto = new CreateUserDto(request.FirstName, request.LastName, request.Email, request.Password);

            var userId = await _authentificationService.CreateUserAsync(createUserDto, cancellationToken);

            var emailToken = await _authentificationService.GenerateEmailActivationTokenAsync(userId, cancellationToken);

            // On each Register a notificationSetting is added to the notificationsettings Table
            var notificationSetting = new NotificationSetting()
            {
                Id = Guid.NewGuid(),
                IsApplicationEnabled = false,
                IsEmailEnabled = false,
                UserId = userId,
                CreatedOn = DateTimeOffset.Now,
                CreatedByUserId = userId,
                IsDeleted = false,
            };
            await _applicationDbContext.NotificationSettings.AddAsync(notificationSetting, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);


            var fullName = $"{request.FirstName} {request.LastName}";

            var jwtDto = _jwtService.Generate(userId, request.FirstName, request.LastName, fullName);


            var name = $"-BUTTONNAME-";

            name.Replace("BUTTONNAME-", "Hesabinizi aktiflestirmek icin tiklayiniz.");

            // Email is sent after each register<
            _emailService.SendEmailConfirmation(new SendEmailConfirmationDto()

            {
                Email = request.Email,
                Name = request.FirstName,
                Token = emailToken
            }); ;

            return new AuthRegisterDto(request.Email, fullName, jwtDto.AccessToken);


        }
    }
}
