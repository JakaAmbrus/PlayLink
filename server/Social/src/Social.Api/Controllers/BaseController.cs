using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Security;

namespace Social.Api.Controllers
{
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
