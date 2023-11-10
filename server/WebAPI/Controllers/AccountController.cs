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

            if (result?.Success == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> Login(UserLoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (result?.Success == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
// Before using identity I made my own register and login methods, I will leave them here in case I decide to use them again in the future

/*[HttpPost("register")]
public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
{
    if (await UserExists(registerDto.Username)) return BadRequest("Username already in use");

    using var hmac = new HMACSHA512();

    var user = new AppUser
    {
        UserName = registerDto.Username.ToLower(),
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        PasswordSalt = hmac.Key
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return new UserDto
    {
        Username = user.UserName,
        Token = _tokenService.CreateToken(user)
    };
}*/


/*[HttpPost("login")]
public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
{
    var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

    if (user == null) return Unauthorized();

    using var hmac = new HMACSHA512(user.PasswordSalt);

    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

    for (int i = 0; i < computedHash.Length; i++)
    {

        if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
    }

    return new UserDto
    {
        Username = user.UserName,
        Token = _tokenService.CreateToken(user)
    };
}*/