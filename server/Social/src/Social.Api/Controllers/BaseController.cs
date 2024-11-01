using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Authentication.Interfaces;
using Social.Api.Filters;

namespace Social.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize(Policy = "RequireMemberRole")]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private ISender _mediator;
        private IAuthContextService _authService;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        protected IAuthContextService AuthService => _authService ??= HttpContext.RequestServices.GetRequiredService<IAuthContextService>();
    }
}
