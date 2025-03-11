using Microsoft.AspNetCore.Mvc;
using OnlineMarket.Domain.Common;

namespace OnlineMarket.Api.Helpers
{
    public static class ResultExtensions
    {
        public static IActionResult FromResultCode<T>(this ControllerBase controller, Result<T> result)
        {
            return result.Status switch
            {
                ResultStatus.Success => controller.Ok(result.Value),
                ResultStatus.NotFound => controller.NotFound(result.ErrorMessage),
                ResultStatus.BadRequest => controller.BadRequest(result.ErrorMessage),
                ResultStatus.Forbidden => controller.Forbid(),
                _ => controller.StatusCode(500, "An unknown error occurred.")
            };
        }
    }
}
