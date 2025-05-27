// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Core.Api.Filters;

public class LogActionArgumentsFilter(ILogger<LogActionArgumentsFilter> _logger) : IActionFilter
{
  public void OnActionExecuted(ActionExecutedContext context)
  {
    if (!_logger.IsEnabled(LogLevel.Debug))
    {
      return; // No need to log if debug level is not enabled
    }

    string methodName = context?.ActionDescriptor?.DisplayName ?? "Unknown Method";
    _logger.LogDebug("Sending response from {Method}...", methodName);
  }

  public void OnActionExecuting(ActionExecutingContext context)
  {
    if (! _logger.IsEnabled(LogLevel.Debug))
    {
      return; // No need to log if debug level is not enabled
    }

    string methodName = context?.ActionDescriptor?.DisplayName ?? "Unknown Method";
    string arguments = context?.ActionArguments is null
      ? string.Empty
      : string.Join(", ", context.ActionArguments.Select(a => $"{a.Key}: {a.Value}"));
    _logger.LogDebug("Receiving request for {Method}({Arguments})...", methodName, arguments);
  }
}
