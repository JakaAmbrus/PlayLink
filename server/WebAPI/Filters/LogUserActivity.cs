using Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUserActivityService _userActivityService;
        public LogUserActivity(IAuthenticatedUserService authenticatedUserService, IUserActivityService userActivityService)
        {
            _authenticatedUserService = authenticatedUserService;
            _userActivityService = userActivityService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

        var resultContext = await next();

        if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

        var userId = _authenticatedUserService.UserId;
        
        await _userActivityService.UpdateLastActiveAsync(userId);


        }
    }
}
