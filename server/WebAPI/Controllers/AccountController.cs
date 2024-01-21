using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Authentication.UserLogin;
using Application.Features.Authentication.UserRegistration;
using Application.Features.Authentication.GuestUserLogin;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Handles user registration and login of users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ISender _mediator;

        public AccountController(ISender sender)
        {
            _mediator = sender;
        }

        /// <summary>
        /// Registers a new user and adds them to the Database.
        /// </summary>
        /// <param name="command">Required information for successful registration: Username, Password, Gender, Full Name, Country and Date of birth.</param>
        /// <returns>JWT and username.</returns>
        [HttpPost("register")] 
        public async Task<IActionResult> Register(UserRegistrationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Logs in a user and returns a JWT.
        /// </summary>
        /// <param name="command">Username and Password.</param>
        /// <returns>JWT and username.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Logs in a guest user and returns a JWT.
        /// </summary>
        /// <param name="role">The role of the guest user account(member or moderator).</param>
        /// <returns>JWT and username.</returns>
        [HttpPost("guest-login/{role}")]
        public async Task<IActionResult> GuestLogin(string role, CancellationToken cancellationToken)
        {
            var command = new GuestUserLoginCommand{ Role = role };

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }
    }
}