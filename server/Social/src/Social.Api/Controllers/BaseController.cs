using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Security;
using Social.Api.Filters;

namespace Social.Api.Controllers
{
    // [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [Authorize(Policy = "Member")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private ISender _mediator;
        private IAuthContextService _authService;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        protected IAuthContextService AuthService => _authService ??= HttpContext.RequestServices.GetRequiredService<IAuthContextService>();
    }
}
