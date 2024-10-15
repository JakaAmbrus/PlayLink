﻿using Social.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private IAuthService _authService;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        protected IAuthService AuthService => _authService ??= HttpContext.RequestServices.GetRequiredService<IAuthService>();
    }
}