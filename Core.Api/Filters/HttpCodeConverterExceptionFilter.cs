// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Api.Filters;

public class HttpCodeConverterExceptionFilter
  (ILogger<HttpCodeConverterExceptionFilter> _logger
  , IHostEnvironment _hostEnvironment
  , ProblemDetailsFactory _problemDetailsFactory)
  : IExceptionFilter
{
  public void OnException(ExceptionContext context)
  {
    bool isDevelopment = _hostEnvironment.IsDevelopment();

    if (context.Exception is ArgumentException)
    {
      _logger.LogError(context.Exception, "Bad request");

      context.Result = new BadRequestResult();
      context.ExceptionHandled = !isDevelopment; // In development, we want to throw the exception to see the stack trace, otherwise
      return;
    }

    _logger.LogError(context.Exception, "Internal error");

    var problemDetails = _problemDetailsFactory.CreateProblemDetails(
     context.HttpContext,
     statusCode: StatusCodes.Status500InternalServerError,
     title: "An unexpected error occurred",
     detail: context.Exception.Message
     );

    context.Result = new ObjectResult(problemDetails)
    {
      StatusCode = StatusCodes.Status500InternalServerError
    };

    context.ExceptionHandled = !isDevelopment; // In development, we want to throw the exception to see the stack trace, otherwise
  }
}
