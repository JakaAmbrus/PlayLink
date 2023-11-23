using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public LogUserActivity(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

        var resultContext = await next();

        if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

        var userId = _authenticatedUserService.UserId;


        }
    }
}
