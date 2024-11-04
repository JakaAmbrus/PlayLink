using Social.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Core.Security;

namespace Social.Api.Filters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        private readonly IAuthContextService _authService;
        private readonly IUserActivityService _userActivityService;
        public LogUserActivity(IAuthContextService authService, IUserActivityService userActivityService)
        {
            _authService = authService;
            _userActivityService = userActivityService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var resultContext = await next();

            if (resultContext.HttpContext.User.Identity != null && resultContext.HttpContext.User.Identity.IsAuthenticated && resultContext.Exception == null)
            {
                var userId = _authService.GetSocialId();
                await _userActivityService.UpdateLastActiveAsync(userId);
            }
        }
    }
}
