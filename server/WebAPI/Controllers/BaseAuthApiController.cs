using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize(Policy = "RequireMemberRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAuthApiController : ControllerBase
    {
        protected readonly ISender Mediator;
        protected readonly IAuthenticatedUserService AuthenticatedUserService;

        public BaseAuthApiController(ISender mediator, IAuthenticatedUserService authenticatedUserService)
        {
            Mediator = mediator;
            AuthenticatedUserService = authenticatedUserService;
        }
        protected int GetCurrentUserId()
        {
            return AuthenticatedUserService.UserId;
        }

        protected IEnumerable<string> GetCurrentUserRoles()
        {
            return AuthenticatedUserService.UserRoles;
        }
    }
}
