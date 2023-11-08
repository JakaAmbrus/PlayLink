using WebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Interfaces;
using WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using WebAPI.Features.Common;
using WebAPI.Features.Register;
using MediatR;

namespace WebAPI.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly ISender _mediator;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, ISender sender)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mediator = sender;
        }

        [HttpPost("register")] 
        public async Task<ActionResult<RegisterResponse>> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (result?.Success == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower().Trim());

            if (user == null) return Unauthorized("invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) return Unauthorized("invalid password");

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {

            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
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