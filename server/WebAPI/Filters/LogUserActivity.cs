using Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        private readonly IAuthService _authService;
        private readonly IUserActivityService _userActivityService;
        public LogUserActivity(IAuthService authService, IUserActivityService userActivityService)
        {
            _authService = authService;
            _userActivityService = userActivityService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var resultContext = await next();

            if (resultContext.HttpContext.User.Identity != null && resultContext.HttpContext.User.Identity.IsAuthenticated && resultContext.Exception == null)
            {
                var userId = _authService.GetCurrentUserId();
                await _userActivityService.UpdateLastActiveAsync(userId);
            }
        }
    }
}
