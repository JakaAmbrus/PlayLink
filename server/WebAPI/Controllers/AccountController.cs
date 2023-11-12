using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Authentication.UserLogin;
using Application.Features.Authentication.UserRegistration;

namespace WebAPI.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly ISender _mediator;

        public AccountController(ISender sender)
        {
            _mediator = sender;
        }

        [HttpPost("register")] 
        public async Task<ActionResult<UserRegistrationResponse>> Register(UserRegistrationCommand command)
        {
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> Login(UserLoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}