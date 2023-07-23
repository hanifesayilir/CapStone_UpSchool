using Application.Common.Models.Errors;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace WebApi.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            ApiErrorDto apiErrorDto = new ApiErrorDto();

            switch (context.Exception)
            {
                case ValidationException:

                    var validationException = context.Exception as ValidationException;

                    var propertyNames = validationException.Errors
                        .Select(x => x.PropertyName)
                        .Distinct();

                  

                    foreach (var propertyName in propertyNames)
                    {
                        var propertyFailures = validationException.Errors
                            .Where(e => e.PropertyName == propertyName)
                            .Select(x => x.ErrorMessage)
                            .ToList();


                        apiErrorDto.Errors.Add(new ErrorDto(propertyName, propertyFailures));
                    }

                    apiErrorDto.Message = "One or more validation errors were occurred.";

                    context.Result = new BadRequestObjectResult(apiErrorDto);
                    break;



                default:

                    _logger.LogError(context.Exception, context.Exception.Message);

                 

                    apiErrorDto.Message = "An unexpected error was occurred.";

                    context.Result = new ObjectResult(apiErrorDto)
                    {
                        StatusCode = (int)StatusCodes.Status500InternalServerError
                    };

                    break;
            }

            return Task.CompletedTask;
        }
    }
}
