using Application.Common.Interfaces;
using Application.Common.Models.Email;
using Application.Features.Notifications.Queries.GetByUserId;
using Domain.Common;
using Domain.Entities;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace Application.Features.Products.Commands.AddList
{
    public class ProductListAddCommandHandler : IRequestHandler<ProductListAddCommand, Response<Guid>>
    {

        private readonly IApplicationDbContext _applicationDbContext;
       
        private readonly IEmailService _emailService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly INotificationApplicationHubService _notificationApplicationHubService;

        public ProductListAddCommandHandler(IApplicationDbContext applicationDbContext, IEmailService emailService, INotificationApplicationHubService notificationApplicationHubService, ICurrentUserService currentUserService,  UserManager<User> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _emailService = emailService;
            _notificationApplicationHubService = notificationApplicationHubService;
            _currentUserService = currentUserService;
            _userManager = userManager;
         

        }

        
            public async Task<Response<Guid>> Handle(ProductListAddCommand request, CancellationToken cancellationToken)
            {
            try
            {
                List<Product> productList = new List<Product>();
                Guid orderId;

         

                foreach (var product in request.Products)
                {
                    
                    if (product == null) continue;
                    else
                    {
                        Product productNew = new Product()
                        {
                            OrderId = product.OrderId,
                            Name = product.Name,
                            IsOnSale = product.IsOnSale,
                            Picture = product.Picture,
                            Price = product.Price,
                            SalePrice = product.SalePrice,
                            CreatedOn = DateTimeOffset.Now,
                            CreatedByUserId = null,
                            IsDeleted = false,
                        };

                       
                        productList.Add(productNew);
                    }
                }

             

                await _applicationDbContext.Products.AddRangeAsync(productList, cancellationToken);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);


                
                var notificationSetting = await _applicationDbContext.NotificationSettings.Where(x =>x.UserId == _currentUserService.UserId).FirstOrDefaultAsync();

           
               

                if (notificationSetting != null && notificationSetting.IsEmailEnabled)
                {
                   
                    var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId) ;

                    string tempUserName = $"{currentUser.FirstName}  {currentUser.LastName}";

                    _emailService.SendEmailNotification(new SendEmailNotificationDto()
                    {
                        Email = currentUser.Email,
                        Name = tempUserName,
                        OrderNumber=1,

                    });

                }

                // send application message to wasm notificationpage
                if (notificationSetting != null && notificationSetting.IsApplicationEnabled)
                {
                   

                    var notification = new Notification()
                    {
                        Id = Guid.NewGuid(),
                        IsChecked = false,
                        UserId = _currentUserService.UserId,
                        NotificationType = Domain.Enums.NotificationType.Application,
                        Content = $"{productList.Count} products are added successfully in your order. Your crawler and order are completed." ,
                        CreatedOn = DateTimeOffset.UtcNow,
                        CreatedByUserId = _currentUserService.UserId,
                    };

                    await _applicationDbContext.Notifications.AddAsync(notification, cancellationToken);
                    await _applicationDbContext.SaveChangesAsync(cancellationToken);

                    await _notificationApplicationHubService.SendApplication(new NotificationGetByUserIdDto()
                    {
                        Id = notification.Id,
                        Content = notification.Content,
                        IsChecked = notification.IsChecked,
                        NotificationType = notification.NotificationType,
                        CreatedOn = notification.CreatedOn,
                    }, cancellationToken);



                }

                return new Response<Guid>($"{productList.Count} products were added successfully");
            } catch(Exception ex)
            {
                Console.WriteLine("ExceptionMessage: " +ex.Message);
                return null;
            }
                

            }

        private NotificationGetByUserIdDto MapToNotificationGetByUserIdDto(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
    }

