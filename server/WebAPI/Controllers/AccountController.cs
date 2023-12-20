using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Authentication.UserLogin;
using Application.Features.Authentication.UserRegistration;

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
        /// </summary>https://desktop.postman.com/?desktopVersion=10.20.0&userId=30655445&teamId=0
        /// <param name="command">Required information for successful registration: Username, Password, Gender, Full Name, Country and Date of birth.</param>
        /// <returns>JWT and username.</returns>
        [HttpPost("register")] 
        public async Task<IActionResult> Register(UserRegistrationCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Logs in a user and returns a JWT.
        /// </summary>
        /// <param name="command">Username and Password.</param>
        /// <returns>JWTand username.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}